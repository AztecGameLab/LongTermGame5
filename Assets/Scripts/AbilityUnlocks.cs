using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlocks : Singleton<AbilityUnlocks>
{
    public enum Abilities {Slide, ReflectingProjectile, Bash, FireBall, DoubleJump, FreezeProjectile, GroundPound, Grapple}
    
    public void Unlock(Abilities ability)
    {
        switch (ability)
        {
            case Abilities.Slide:
                GetComponent<SlideAbility>().enabled = true;
                break;
            case Abilities.ReflectingProjectile:
                print("unlock");
                break;
            case Abilities.Bash:
                GetComponent<BashAbility>().enabled = true;
                break;
            case Abilities.FireBall:
                print("unlock");
                break;
            case Abilities.DoubleJump:
                GetComponent<PlatformerController>().parameters.JumpCount = 2;
                break;
            case Abilities.FreezeProjectile:
                print("unlock");
                break;
            case Abilities.GroundPound:
                GetComponent<GroundPound>().enabled = true;
                break;
            case Abilities.Grapple:
                print("unlock");
                break;
        }
    }
}
