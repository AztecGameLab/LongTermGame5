using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerController : Entity
{
    [SerializeField] private PlatformerParameters parameters;
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Animator anim;
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
            rigid.freezeRotation = true;
        }

        PhysicsMaterial2D material = new PhysicsMaterial2D();
        material.friction = 0;
        this.GetComponent<Collider2D>().sharedMaterial = material;

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
    }

    void Update(){
        //anim.SetFloat("HorizontalSpeed", rigid.velocity.x);
        //anim.SetFloat("VerticalSpeed", rigid.velocity.y);

        if(Mathf.Abs(rigid.velocity.x) > 0){
            render.flipX = rigid.velocity.x > 0;
        }
    }

    [SerializeField] float error; //For testing purposes
    void FixedUpdate(){
        //Going with a PID loop with only P lol
        if(lockControls){ return; }
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
    }

    #region Jumping
    public void OnJumpPerformed(InputAction.CallbackContext context){
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
    public void ProjectileHandler(InputAction.CallbackContext context){
        //if(weapons == null){ return; }
        //if(weapons.Count <= 0){ return; }
        
        int activeWeapon = currWeapon;
        if(context.performed){
            //Time.timeScale = parameters.BulletTimeSlowDown;
            //print("AAAAAAAHHHHHHHhh Running");
            //weapons[activeWeapon].Charge();
        }else if(context.canceled){
            //Time.timeScale = 1; //Return to regular timescale
            //weapons[activeWeapon].Fire();
        }
    }

    #endregion
    
    public void Interact(InputAction.CallbackContext context)
    {
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
        isJumping = false;
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

    public Collider2D attackTrigger;
    bool attacking = false;
    public void AttackHandler(InputAction.CallbackContext context){
        if(context.started){
            attacking = true;
        } else if(context.canceled){
            attacking = false;
        }
    }


    #endregion


    #region EntityStuff

    public override void TakeDamage(float baseDamage, Vector2 direction){
        //TODO :: APPLY KNOCKBACK

        //NOTE :: NOT NORMALIZED
        base.TakeDamage(baseDamage);
    }

    public override void OnDeath()
    {
        //We don't want to destroy ourselves on death lmao
        return;
    }

    #endregion
}
