using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedAura : Entity
{
    public override void TakeDamage(float baseDamage)
    {
        var controller = PlatformerController.instance;
        var shakeIntensity = 0.2f;
        controller.playerImpulseSource.GenerateImpulse(shakeIntensity);
        
        base.TakeDamage(baseDamage);
    }

    public override void OnDeath()
    {
        var controller = PlatformerController.instance;
        var shakeIntensity = 1f;
        controller.playerImpulseSource.GenerateImpulse(shakeIntensity);
        
        base.OnDeath();
    }
}
