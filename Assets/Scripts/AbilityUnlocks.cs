using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlocks : Singleton<AbilityUnlocks>
{
    public static event Action<Abilities> AbilityUnlocked;
    
    public enum Abilities {Slide, ReflectingProjectile, Bash, FireBall, DoubleJump, FreezeProjectile, GroundPound, Grapple, None}
    
    [EasyButtons.Button]
    public void Unlock(Abilities ability)
    {
        var pc = GetComponent<PlatformerController>();
        
        print(ability);
        switch (ability)
        {

            case Abilities.Slide:
                print("addslide");
                gameObject.AddComponent<SlideAbility>();
                PlatformerController.instance.currentUnlockState = 1;
                break;
            case Abilities.ReflectingProjectile:
                print("addrico");
                pc.weapons.Add(Resources.Load<Ricochet>("Weapons/Ricochet"));
                pc.currWeapon = pc.weapons.Count - 1;
                PlatformerController.instance.currentUnlockState = 2;
                break;
            case Abilities.Bash:
                print("addbash");
                gameObject.AddComponent<BashAbility>();
                PlatformerController.instance.currentUnlockState = 3;
                break;
            case Abilities.FireBall:
                print("unlock");
                pc.weapons.Add(Resources.Load<Ricochet>("Weapons/FireBall"));
                pc.currWeapon = pc.weapons.Count - 1;
                PlatformerController.instance.currentUnlockState = 4;
                break;
            case Abilities.DoubleJump:
                print("unlock");
                GetComponent<PlatformerController>().parameters.JumpCount = 2;
                PlatformerController.instance.currentUnlockState = 5;
                break;
            case Abilities.FreezeProjectile:
                print("unlock");
                pc.weapons.Add(Resources.Load<Ricochet>("Weapons/FreezeProjectile"));
                pc.currWeapon = pc.weapons.Count - 1;
                PlatformerController.instance.currentUnlockState = 6;
                break;
            case Abilities.GroundPound:
                gameObject.AddComponent<GroundPound>();
                PlatformerController.instance.currentUnlockState = 7;
                break;
            case Abilities.Grapple:
                print("unlock");
                pc.weapons.Add(Resources.Load<Ricochet>("Weapons/GrappleHook"));
                pc.currWeapon = pc.weapons.Count - 1;
                PlatformerController.instance.currentUnlockState = 8;
                break;
            case Abilities.None:
                break;
        }
        
        AbilityUnlocked?.Invoke(ability);
    }
}
