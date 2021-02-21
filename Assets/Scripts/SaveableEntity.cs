using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableEntity : MonoBehaviour
{
    public string id = string.Empty;

    [ContextMenu("Generate ID")]
    private void GenerateId() => id = System.Guid.NewGuid().ToString();

    private void Reset()
    {
        GenerateId();
    }

    public object CaptureComponentStates()
    {
        var state = new Dictionary<string, object>();

        foreach (var saveable in GetComponents<ISaveable>())
        {
            state[saveable.GetType().ToString()] = saveable.CaptureState();
        }

        return state
    }

    public void RestoreComponentStates(object state)
    {
        var stateDictionary = (Dictionary<string, object>)state;

        foreach (var saveable in GetComponents<ISaveable>())
        {
            string typeName = saveable.GetType().ToString();
            if(stateDictionary.TryGetValue(typeName, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
}
