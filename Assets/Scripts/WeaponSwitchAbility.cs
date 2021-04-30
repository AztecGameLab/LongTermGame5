using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitchAbility : Ability
{

    //My reasoning for a weaponWheel
    //https://uxdesign.cc/the-evolution-of-accessibility-weapons-wheel-f8fed0fed78e

    protected override string InputName => "WeaponWheel";

    public WeaponWheelUI WeaponWheel;

    public float quickStopTimeout = 0.5f;

    public int lastWeapon = 0;

    protected override void Started(InputAction.CallbackContext context) { 

    }

    protected override void Canceled(InputAction.CallbackContext context) {
        //This will run when the button is released
        
        print("Button Released");

        //I was lazy
        int weaponIndex = Mathf.RoundToInt(((Vector2.SignedAngle(new Vector2(1, 0), Player.primaryStick))+90)/90);
            if(weaponIndex == -1)
                weaponIndex = 3;

        StopCoroutine(UpdateInput());
        WeaponWheel.Disappear();
        Player.TimeSlowDown(false);

        //Quickswap or you chose a weapon that you dont have
        if(weaponIndex > Player.weapons.Count){
            return;
        }
        Player.weapons[Player.currWeapon].Cancel();
        Player.currWeapon = weaponIndex;
    }

    protected override void Performed(InputAction.CallbackContext context) {
        //This will run when when the InputAction is held for n amount of time
        Player.TimeSlowDown(true);
        WeaponWheel.Appear();
        StartCoroutine(UpdateInput());
    }

    IEnumerator UpdateInput(){
        while(WeaponWheel.enabled){
            //I was lazy
            int weaponIndex = Mathf.RoundToInt(((Vector2.SignedAngle(new Vector2(1, 0), Player.primaryStick))+90)/90);
            if(weaponIndex == -1)
                weaponIndex = 3;
                
            WeaponWheel.HighlightWeapon(weaponIndex);
            yield return null;
        }
    }
}
