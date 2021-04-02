using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlideAbility : Ability
{
    protected override string InputName => "Slide";
    private Animator anim;

    [Tooltip("How much speed do we get from a slide")]
    public float slideSpeed = 15;

    [Tooltip("Duration of the slide (sec)")]
    public float slideDuration = 1;

    [Tooltip("How big does the collider shirink when sliding (%)")]
    public float shrinkValue = .25f;

    protected override void Awake(){
        base.Awake();
    }

    protected override void Started(InputAction.CallbackContext context){
        StartCoroutine(Sliding());
    }

    IEnumerator Sliding(){
        Player.lockControls = true;
        
        while((slideDuration -= Time.deltaTime) >= 0){
            Player.rigid.AddForce(Vector2.right * Player.facingDirection * slideSpeed);
            yield return null;
        }
        Player.lockControls = false;

    }
}
