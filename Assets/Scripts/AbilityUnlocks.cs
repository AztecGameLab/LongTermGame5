using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlocks : Singleton<AbilityUnlocks>
{
    public static event Action<Abilities> AbilityUnlocked;
    
    public enum Abilities {Slide, ReflectingProjectile, Bash, FireBall, DoubleJump, FreezeProjectile, GroundPound, Grapple, None}
    
    public void Unlock(Abilities ability)
    {
        switch (ability)
        {
            case Abilities.None:
                break;
            case Abilities.Slide:
                gameObject.AddComponent<SlideAbility>();
                break;
            case Abilities.ReflectingProjectile:
                var pc = GetComponent<PlatformerController>();
                pc.weapons.Add(Resources.Load<Ricochet>("Weapons/Ricochet"));
                pc.currWeapon = pc.weapons.Count - 1;
                break;
            case Abilities.Bash:
                gameObject.AddComponent<BashAbility>();
                break;
            case Abilities.FireBall:
                print("unlock");
                break;
            case Abilities.DoubleJump:
                print("unlock");
                //GetComponent<PlatformerController>().parameters.JumpCount = 2;
                break;
            case Abilities.FreezeProjectile:
                print("unlock");
                break;
            case Abilities.GroundPound:
                gameObject.AddComponent<GroundPound>();
                break;
            case Abilities.Grapple:
                print("unlock");
                break;
        }
        
        AbilityUnlocked?.Invoke(ability);
    }
}
