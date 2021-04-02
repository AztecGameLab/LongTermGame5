using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlideAbility : Ability
{
    protected override string InputName => "Slide";
    private Animator anim;

    [Tooltip("Duration of the slide (sec)")]
    public float slideDuration = 1;

    [Tooltip("How big does the collider shirink when sliding (%)")]
    public float shrinkValue = .25f;

    [Tooltip("How fast should our slide speed be? (% of max speed)")]
    public float slideSpeedMultiplier = 3f;

    [Tooltip("How long until we can slide again")]
    public float slideCooldown = .25f;

    [SerializeField] bool canSlide = true;
    protected override void Started(InputAction.CallbackContext context){
        if(!canSlide)
            return;

        int direction = Player.primaryStick.x >= 0 ? 1 : -1;
        if(Player.primaryStick.x == 0){
            direction = Player.facingDirection;
        }

        float goalVelocity = direction * Player.parameters.MaxRunSpeed * slideSpeedMultiplier;

        float velocityToAdd = goalVelocity - Player.rigid.velocity.x; //HAHA no upper limit
        
        StartCoroutine(Sliding());
        Player.rigid.AddForce((Vector2.right) * (velocityToAdd * Player.rigid.mass), ForceMode2D.Impulse);
    }

    IEnumerator Sliding(){
        canSlide = false;
        Player.lockControls = true;
        Player.coll.bounds.size.Set(Player.coll.bounds.size.x, Player.coll.bounds.size.y * shrinkValue, Player.coll.bounds.size.z);
        yield return new WaitForSeconds(slideDuration);
        StartCoroutine(Cooldown());
        Player.coll.bounds.size.Set(Player.coll.bounds.size.x, Player.coll.bounds.size.y / shrinkValue, Player.coll.bounds.size.z);
        Player.lockControls = false;
    }

    IEnumerator Cooldown(){
        canSlide = false;
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
    }
}
