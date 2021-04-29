using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodEvent : MonoBehaviour
{
    bool usedGodEvent;
    public AbilityUnlocks.Abilities abilityToUnlock;

    public void StartGodEvent()
    {
        if(usedGodEvent)
            return;

        PlatformerController.instance.canTakeDamage = false;
        PlatformerController.instance.lockControls = true;
        DialogSystem dialogSystem = GetComponentInChildren<DialogSystem>();
        dialogSystem.StartDialog();
        dialogSystem.finishedDialog = () => UnlockAbility(abilityToUnlock);
    }

    void UnlockAbility(AbilityUnlocks.Abilities abilityToUnlock)
    {
        AbilityUnlocks.Get().Unlock(abilityToUnlock);
        usedGodEvent = true;
        PlatformerController.instance.lockControls = false;
        PlatformerController.instance.canTakeDamage = true;
    }
}
