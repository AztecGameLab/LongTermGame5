using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerController : MonoBehaviour
{
    [SerializeField] private PlatformerParameters parameters;
    
    [SerializeField] private Rigidbody2D rigid;
    
    [SerializeField] private Animator anim;
    
    public Interactable interactable;

    SpriteRenderer render;

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
        //Pretty sure there was a reason as to why I don't usually do this
        //But I can't remember for the life of me why

        _instance = this;
        rigid = this.GetComponent<Rigidbody2D>();
        if(rigid == null){
            rigid = this.gameObject.AddComponent<Rigidbody2D>();
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
    }

    void Update(){
        //anim.SetFloat("HorizontalSpeed", rigid.velocity.x);
        //anim.SetFloat("VerticalSpeed", rigid.velocity.y);

        if(Mathf.Abs(rigid.velocity.x) > 0){
            render.flipX = rigid.velocity.x > 0;
        }

        if(jumpCounter >= parameters.JumpCount){
            canJump = false;
        }
    }

    void FixedUpdate(){
        rigid.velocity = new Vector2(goalVelocity, rigid.velocity.y + fastFall);
    }
    
    float fastFall = 0;
    float goalVelocity;
    public void OnMovementChanged(InputAction.CallbackContext context){
        Vector2 movement = context.ReadValue<Vector2>();
        float horizontalVelocity = movement.x * parameters.MaxRunSpeed;
        goalVelocity = horizontalVelocity;

        //TODO :: ADD ACCELLERATION / DECELERATION

        //TODO :: Implement a fast fall function
    }

    public void OnJumpPerformed(InputAction.CallbackContext context){
        if(context.performed)
            StartCoroutine(JumpQueue(parameters.JumpBufferTime));
        else if(context.canceled){
            if(isJumping){
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
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

    public void Jump(){
        rigid.velocity = new Vector2(rigid.velocity.x, parameters.JumpSpeed);
        jumpCounter++;
        StartCoroutine(JumpTimeout());
    }

    IEnumerator CoyoteTime(float time){
        yield return new WaitForSeconds(time);
        jumpCounter++;
    }

    IEnumerator JumpTimeout(){
        //Calculate Time at apex
        //t = (Vf - Vi) / a
        float time = -parameters.JumpSpeed / Physics2D.gravity.magnitude;
        yield return new WaitForSeconds(time);
        isJumping = false;
    }

    int jumpCounter = 0;
    bool isJumping = false;
    bool isGrounded = false;
    bool canJump = false;
    
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
        foreach(ContactPoint2D point in other.contacts){
            Vector2 direction =  point.point - (Vector2)this.transform.position;
            if(Vector3.Angle(-this.transform.up, direction) < parameters.MaxGroundAngle){
                StopCoroutine("CoyoteTime");
                StartCoroutine(CoyoteTime(parameters.CoyoteTime));
                break; //if we know we are grounded, no need to continue checking the loop
            }
        }
    }

    public void Interact(InputAction.CallbackContext context){

        //Run the interact function on the current working interacable that we are working with
        if(interactable != null){
            interactable.OnInteract();
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        isJumping = false;
        CheckGroundedState(other);
    }

    void OnCollisionStay2D(Collision2D other){
        CheckGroundedState(other);
    }

    void OnCollisionExit2D(Collision2D other){
        
    }

    void OnTriggerStay2D(Collider2D other){

        //This is dealing with the interactables
        //Basically we want the "Active" interactable
        //to be the one that is the closest to the player
        if(other.tag == "Interactable"){
            if(interactable == null){
                interactable = other.GetComponent<Interactable>();
            }

            float distNew = Vector3.Distance(this.transform.position, other.transform.position);
            float distCurr = Vector3.Distance(this.transform.position, interactable.transform.position);

            if(distNew > distCurr){
                interactable = other.GetComponent<Interactable>();
            }

            interactable.OnHover();
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.Equals(interactable.gameObject)){
            interactable = null;
        }
    }
}
