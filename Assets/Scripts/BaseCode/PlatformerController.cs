using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerController : Entity
{
    [SerializeField] public PlatformerParameters parameters;
    [SerializeField] public Rigidbody2D rigid;
    [SerializeField] private Animator anim;
    [SerializeField] private float deathTimeSeconds = 2f;
    
    public bool isDying = false;
    private UiController _uiController;
    
    public CinemachineImpulseSource playerImpulseSource;
    public CapsuleCollider2D coll;
    SpriteRenderer render;

    public bool lockControls = false;
    public GameInputs Inputs;

    public int currWeapon;
    public List<ProjectileWeapon> weapons;
    private static PlatformerController _instance;
    public static PlatformerController instance{
        get{
            if(_instance == null){
                _instance = GameObject.FindObjectOfType<PlatformerController>();
            }
            return _instance;
        }
    }

    void Awake(){
        _instance = this;
        rigid = this.GetComponent<Rigidbody2D>();
        if(rigid == null){
            rigid = this.gameObject.AddComponent<Rigidbody2D>();
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigid.sharedMaterial = new PhysicsMaterial2D();
            rigid.sharedMaterial.friction = 0;
            rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigid.interpolation = RigidbodyInterpolation2D.Extrapolate;
            rigid.gravityScale = 3;
            rigid.freezeRotation = true;
        }

        this.transform.tag = "Player";

        PhysicsMaterial2D material = new PhysicsMaterial2D();
        material.friction = 0;
        coll = this.GetComponent<CapsuleCollider2D>();
        coll.sharedMaterial = material;

        anim = this.GetComponent<Animator>();
        if(anim == null){
            anim = this.gameObject.AddComponent<Animator>();
        }

        render = this.GetComponent<SpriteRenderer>();
        if(render == null){
            render = this.gameObject.AddComponent<SpriteRenderer>();
        }

        Inputs = new GameInputs();
        Inputs.Enable();

        if(parameters == null){
            Debug.Log("Error!!, no platformer parameter!!");
            parameters = Resources.Load<PlatformerParameters>("PlatformerParameters"); //If we dont have one try and load it
        }

        health = parameters.MaxHealth;
        _uiController = UiController.Get();
        UiController.Get().SetHealth(health/parameters.MaxHealth);
    }

    public int facingDirection = 1;
    void Update(){
        //anim.SetFloat("HorizontalSpeed", rigid.velocity.x);
        //anim.SetFloat("VerticalSpeed", rigid.velocity.y);

        anim.SetFloat("speed", Mathf.Abs(rigid.velocity.x));
        
        if(Mathf.Abs(rigid.velocity.x) > 0.25f){
            render.flipX = !(rigid.velocity.x > 0);
            facingDirection = render.flipX ? -1 : 1;
        }
    }

    [SerializeField] float error; //For testing purposes
    void FixedUpdate(){
        
        if(lockControls || isAiming){ 
            rigid.drag = 2;
            return;
        } else{
            rigid.drag = 0;
        }
        
        //Going with a PID loop with only P lol
        float maxForce = parameters.AccelerationMultiplier * 2.5f;
        error = Mathf.Clamp((goalVelocity - rigid.velocity.x) * parameters.AccelerationMultiplier, -maxForce, maxForce);
        
        #if UNITY_EDITOR
        Debug.DrawLine(this.transform.position, (Vector2)this.transform.position + new Vector2(error, 0));
        #endif

        rigid.AddForce(new Vector2(error, 0));
    }
    public Vector2 primaryStick;
    float fastFall = 0;
    public float goalVelocity;
    public void OnMovementChanged(InputAction.CallbackContext context){
        primaryStick = context.ReadValue<Vector2>();
        float horizontalVelocity = primaryStick.x * parameters.MaxRunSpeed * rigid.mass;
        goalVelocity = horizontalVelocity;

        //TODO :: Implement a fast fall function

        //handle the projectile aiming
        if(primaryStick.sqrMagnitude > Vector2.kEpsilon)
            aimDirection = primaryStick.normalized;
        ProjectileAimHandle(aimDirection);
    }

    #region Jumping
    public void OnJumpPerformed(InputAction.CallbackContext context){
        if(lockControls) return; //Dont allow jumps when locked

        if(context.performed){
            if(jumpCounter >= parameters.JumpCount){
                canJump = false;
            }
            StartCoroutine(JumpQueue(parameters.JumpBufferTime));
        }
        else if(context.canceled){
            if(isJumping){
                if(rigid.velocity.y > 0)
                    rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y / 4);
                isJumping = false;
            }
        }
    }

    //Queue up a jump in the case in which we are unable to jump rn
    //If we are then able to jump in this time then execute a jump
    IEnumerator JumpQueue(float timeout){
        while(timeout > 0){
            yield return new WaitForEndOfFrame();
            timeout -= Time.deltaTime;
            if(!canJump) { continue; } //We can't jump we just wait
            if(isJumping) { continue; } //Can't jump while we are already jumping

            Jump();
            break;
        }
    }

    private void Jump(){
        anim.Play("jump");
        rigid.velocity = new Vector2(rigid.velocity.x, parameters.JumpSpeed);
        jumpCounter++;
        isJumping = true;
        StartCoroutine(JumpTimeout());
    }

    IEnumerator CoyoteTime(float time){
        yield return new WaitForSeconds(time);
        jumpCounter++;
    }

    IEnumerator JumpTimeout(){
        //Calculate Time at apex
        //t = (Vf - Vi) / a
        float time = - parameters.JumpSpeed / Physics2D.gravity.magnitude;
        yield return new WaitUntil(() => rigid.velocity.y <= 0);
        isJumping = false;
    }

    public int jumpCounter = 0;
    public bool isJumping = false;
    public bool isGrounded = false;
    public bool canJump = false;
    
    //This works just fine
    void CheckGroundedState(Collision2D other){
        isGrounded = false;
        foreach(ContactPoint2D point in other.contacts){
            //Get the direction we are currently checking
            Vector2 direction =  point.point - (Vector2)this.transform.position;
            //We're gonna be pretty forgiving and if ANY colliders are below our feet
            if(Vector3.Angle(-this.transform.up, direction) < parameters.MaxGroundAngle){
                isJumping = false;

                if(isJumping){
                    break;
                }

                isGrounded = true;
                canJump = true;
                jumpCounter = 0;

                #if UNITY_EDITOR
                Debug.DrawLine(this.transform.position, this.transform.position + (Vector3)direction * 4, Color.green);
                #endif

                break; //if we know we are grounded, no need to continue checking the loop
            }
        }
    }

    //Almost the same as CheckGroundedState Except this need to be ran in Exit
    public void CheckStartCoyoteTime(Collision2D other){
        if(!isJumping && !isGrounded){
            StopCoroutine("CoyoteTime");
            StartCoroutine(CoyoteTime(parameters.CoyoteTime));
        }
    }

    #endregion

    #region Projectiles

    Vector2 aimDirection = Vector2.zero;
    public bool isAiming = false;
    public void ProjectileHandler(InputAction.CallbackContext context){
        if(lockControls) return; //dont allow projectiles when locked
        if(weapons == null){ return; }
        if(weapons.Count <= 0){ return; }


        if(context.performed && _uiController.GetCurrentFill() > 0){
            weapons[currWeapon].Cancel();
            weapons[currWeapon].Charge(aimDirection);
            AimingState(true);
        }else if(context.canceled && isAiming){
            AimingState(false);
            weapons[currWeapon].Fire(aimDirection);
            _uiController.Consume(weapons[currWeapon].manaCost, true);
            anim.Play("magicCast");
        }
    }

    public void AimingState(bool state){
        isAiming = state;
        TimeSlowDown(state);
    }

    public void TimeSlowDown(bool state){
        if(state){
            Time.timeScale = parameters.BulletTimeSlowDown;
            Time.fixedDeltaTime = .02f * parameters.BulletTimeSlowDown;
        } else {
            Time.timeScale = 1; //Return to regular timescale
            Time.fixedDeltaTime = .02f; //This is default physics time
        }
    }

    public void ProjectileAimHandle(Vector2 direction){
        if(!isAiming) //if we aren't aiming, just ignore
            return;
        if(weapons == null){ return; }
        if(weapons.Count <= 0){ return; }
        weapons[currWeapon].OnAimChange(aimDirection);
    }

    public void ProjectileCancelHandle(InputAction.CallbackContext context){
        if(context.performed){
            CancelProjectile();
        }
    }

    void CancelProjectile(){
        if(weapons == null){ return; }
        if(weapons.Count <= 0){ return; }

        weapons[currWeapon].Cancel();
        AimingState(false);
    }

    #endregion
    
    public void Interact(InputAction.CallbackContext context)
    {
        if(lockControls) return; //Dont allow interacting when locked
            

        if (context.started)
        {
            var nearestInteractable = Scanner.GetClosestObject<Interactable>(transform.position);
        
            if (nearestInteractable != null)
                nearestInteractable.Interact(gameObject);
        }
    }

    public void KnockBack(Vector2 direction, float intensity){
        rigid.AddForce(rigid.mass * direction * intensity, ForceMode2D.Impulse);
    }

    #region Helpers

    void OnCollisionEnter2D(Collision2D other){
        CheckGroundedState(other);
    }

    void OnCollisionStay2D(Collision2D other){
        CheckGroundedState(other);
    }

    void OnCollisionExit2D(Collision2D other){
        CheckGroundedState(other);
        CheckStartCoyoteTime(other);
    }

    #endregion

    #region BasicPunch

    bool attacking = false;
    bool canAttack = true;
    public void AttackHandler(InputAction.CallbackContext context){
        if(lockControls) return; //Dont allow attacking while locked
            
        
        if(canAttack == false) //just ignore the input if we are already attacking
            return;
        
        if(context.started){
            StartCoroutine(AttackTimeout(parameters.BasicAttackDelay, parameters.BasicAttackForgiveness, parameters.BasicAttackCooldown));
        }
    }

    //Fun with booleans lol
    public IEnumerator AttackTimeout(float attackDelay, float attackForgiveness, float attackCooldown){
        float cooldown = attackCooldown - (attackDelay + attackForgiveness);
        canAttack = false;
        yield return new WaitForSeconds(attackDelay);
        attacking = true; //Completly useless boolean

        HashSet<RaycastHit2D> alreadyHits = new HashSet<RaycastHit2D>();

        while((attackForgiveness -= Time.deltaTime) > 0){
            
            anim.Play("punch");
            //I'm gonna be honest, I have no idea if this will work
            //But I guess lets find out
            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, parameters.BasicAttackSize, Vector2.right * facingDirection, parameters.BasicAttackRange);
            HashSet<RaycastHit2D> currentHits = new HashSet<RaycastHit2D>();
            currentHits.UnionWith(hits);
            currentHits.ExceptWith(alreadyHits);
            alreadyHits.UnionWith(hits);
            
            #if UNITY_EDITOR
            Debug.DrawRay(this.transform.position + transform.up * parameters.BasicAttackSize/2, Vector2.right * facingDirection * parameters.BasicAttackRange, Color.red, parameters.BasicAttackForgiveness);
            Debug.DrawRay(this.transform.position - transform.up * parameters.BasicAttackSize/2, Vector2.right * facingDirection * parameters.BasicAttackRange, Color.red, parameters.BasicAttackForgiveness);
            #endif

            foreach(RaycastHit2D hit in currentHits){
                if(hit.transform.CompareTag("Player")) //Just in case don't let us hit ourselves lol
                    continue;

                if(hit.collider.isTrigger)
                    continue;

                Entity e = hit.transform.GetComponent<Entity>();

                if(e == null){
                    //Not an entity
                    continue;
                }

                e.TakeDamage(parameters.BasicBaseDamage, Vector2.right * facingDirection);
            }
        }
        attacking = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    #endregion

    #region EntityStuff
    bool canTakeDamage = true;
    public override void TakeDamage(float baseDamage, Vector2 direction){
        if(!canTakeDamage || DialogSystem.isDialoging)
            return;
        StartCoroutine(KnockBack(direction));
        TakeDamage(baseDamage);
    }

    //Huh, we have no direction to figure out knockback
    //Lets just use a random direction
    public override void TakeDamage(float baseDamage){
        if(!canTakeDamage || DialogSystem.isDialoging)
            return;
        CancelProjectile();
        StartCoroutine(InvincibilityFrames(parameters.InvincibilityTime));
        UiController.Get().SetHealth(health/parameters.MaxHealth);
        base.TakeDamage(baseDamage);
    }

    public IEnumerator KnockBack(Vector2 direction){
        lockControls = true;
        KnockBack(direction, parameters.KnockBackIntensity);
        yield return new WaitForSeconds(parameters.KnockBackTime);
        lockControls = false;
    }

    public IEnumerator InvincibilityFrames(float invincibilityTime){
        canTakeDamage = true;
        Color color = render.color;
        color.a = .5f;
        render.color = color;
        yield return new WaitForSeconds(invincibilityTime);
        color.a = 1;
        render.color = color;
        canTakeDamage = true;
    }

    public override async void OnDeath()
    {
        if (isDying)
            return;
        
        //We don't want to destroy ourselves on death lmao
        anim.Play("death");
        lockControls = true;
        isDying = true;
        
        await Task.Delay(TimeSpan.FromSeconds(deathTimeSeconds));
        print("done waiting");
        LevelUtil.Get().LoadSavedGame();
    }

    #endregion
}