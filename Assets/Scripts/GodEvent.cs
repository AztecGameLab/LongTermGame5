using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystem;

public class GodEvent : MonoBehaviour, ISaveableComponent
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
        dialogSystem.finishedDialog += GoToScene;
        dialogSystem.finishedDialog += () => UnlockAbility(abilityToUnlock);
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
        if (transitionLevel != null)
        {
            print("Transitioning");
            LevelUtil.Get().TransitionTo(transitionLevel);
        }
    }
    
    
    #region SAVE SYSTEM
    [System.Serializable]
    protected class GodEventSaveData : ISaveData //class that is a container for data that will be saved
    {
        public bool used;

        public override string ToString()
        {
            return "used: " + used;
        }
    }

    public ISaveData GatherSaveData() //store current state into the SaveData class
    {
        return new GodEventSaveData { used = usedGodEvent };
    }
    public void RestoreSaveData(ISaveData state) //receive SaveData class and set variables
    {
        var saveData = (GodEventSaveData)state;

        usedGodEvent = saveData.used;
    }
    #endregion
}
