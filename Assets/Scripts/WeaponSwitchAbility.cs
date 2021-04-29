using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitchAbility : Ability
{

    //My reasoning for a weaponWheel
    //https://uxdesign.cc/the-evolution-of-accessibility-weapons-wheel-f8fed0fed78e

    protected override string InputName => "WeaponSwitch";

    public WeaponWheelUI WeaponWheel;
    public int maxWeapons = 4;

    public float quickStopTimeout = 0.5f;

    int lastWeapon = 0;
    bool quickSwap = true;

    protected override void Started(InputAction.CallbackContext context) { 
        //This will run immediatly when the butten is held down
        StartCoroutine(QuickSwapTimeout(quickStopTimeout));
        print("Button Pressed down");
    }

    protected override void Canceled(InputAction.CallbackContext context) {
        //This will run when the button is released
        
        int weaponIndex = (int)(((Vector2.SignedAngle(Vector2.up, Player.primaryStick) + 180)/360) * maxWeapons);

        StopCoroutine(Update());
        WeaponWheel.Disappear();
        Player.TimeSlowDown(false);

        //Quickswap or you chose a weapon that you dont have
        if(quickSwap || weaponIndex > Player.weapons.Count){
            //Just an XOR Swap
            Player.currWeapon ^= lastWeapon;
            lastWeapon ^= Player.currWeapon;
            Player.currWeapon ^= lastWeapon;
            return;
        }

        lastWeapon = Player.currWeapon;
        Player.currWeapon = weaponIndex;
    }

    protected override void Performed(InputAction.CallbackContext context) {
        //This will run when when the InputAction is held for n amount of time
        Player.TimeSlowDown(true);
        WeaponWheel.Appear();
        StartCoroutine(Update());
        print("Button Held down");
    }

    IEnumerator QuickSwapTimeout(float timeout){
        quickSwap = true;
        yield return new WaitForSeconds(timeout);
        quickSwap = false;
    }

    IEnumerator Update(){
        while(true){
            int weaponIndex = (int)(((Vector2.SignedAngle(Vector2.up, Player.primaryStick) + 180)/360) * maxWeapons);
            WeaponWheel.HighlightWeapon(weaponIndex);
            yield return null;
        }
    }
}
