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

        PlatformerController.instance.lockControls = true;
        DialogSystem dialogSystem = GetComponentInChildren<DialogSystem>();
        dialogSystem.StartDialog();
        dialogSystem.finishedDialog = GoToScene;
        dialogSystem.finishedDialog = () => UnlockAbility(abilityToUnlock);
    }

    void UnlockAbility(AbilityUnlocks.Abilities abilityToUnlock)
    {
        AbilityUnlocks.Get().Unlock(abilityToUnlock);
        usedGodEvent = true;
        PlatformerController.instance.lockControls = false;
    }

    public Level transitionLevel;
    [EasyButtons.Button]
    void GoToScene()
    {
        print("T" + transitionLevel);
        if(transitionLevel != null)
            LevelUtil.Get().TransitionTo(transitionLevel);
    }
}
