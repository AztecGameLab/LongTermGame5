using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Component that is added to GameObjects that have other components (that implements ISaveable) that need to be saved
public class SaveableGameObject : MonoBehaviour
{
    public string id = string.Empty;

    [ContextMenu("Generate ID")]
    private void GenerateId() => id = System.Guid.NewGuid().ToString();

    private void Reset()
    {
        GenerateId();
    }

    public Dictionary<string, SaveData> GatherComponentsSaveData()
    {
        var Dict_ComponentTypes_SaveData = new Dictionary<string, SaveData>();

        foreach (var saveableComponent in GetComponents<ISaveableComponent>())
        {
            Dict_ComponentTypes_SaveData[saveableComponent.GetType().ToString()] = saveableComponent.GatherSaveData();
        }

        return Dict_ComponentTypes_SaveData;
    }

    public void RestoreComponentsSaveData(Dictionary<string, SaveData> ComponentTypes_SaveData_Dict)
    {
        foreach (var saveableComponent in GetComponents<ISaveableComponent>())
        {
            string typeName = saveableComponent.GetType().ToString();
            if(ComponentTypes_SaveData_Dict.TryGetValue(typeName, out SaveData saveData))
            {
                saveableComponent.RestoreSaveData(saveData);
            }
        }
    }
}
