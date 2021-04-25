using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponWheelAbility : Ability
{
    [SerializeField] private WeaponWheel wheel;
    
    private Coroutine _wheelShow;
    private Coroutine _wheelHide;
    private Coroutine _wheelAnimate;
    
    protected override void Started(InputAction.CallbackContext context)
    {
        if (_wheelHide != null)
            StopCoroutine(_wheelHide);
        
        _wheelAnimate = StartCoroutine(Animate());
        _wheelShow = StartCoroutine(wheel.Show());
    }

    protected override void Canceled(InputAction.CallbackContext context)
    {
        if (_wheelShow != null)
            StopCoroutine(_wheelShow);
        
        if (_wheelAnimate != null)
            StopCoroutine(_wheelAnimate);
        
        _wheelHide = StartCoroutine(wheel.Hide());
    }

    private IEnumerator Animate()
    {
        
        while (true)
        {
            yield return null;
        }
    }

    protected override string InputName => "WeaponWheel";
}