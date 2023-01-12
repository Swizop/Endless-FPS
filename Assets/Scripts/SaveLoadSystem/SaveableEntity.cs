using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//we have tto paste this to the object we want to save the state
public class SaveableEntity : MonoBehaviour
{
    [SerializeField] private string id;

    public string Id => id;

    [ContextMenu("Generate Id")]
    private void GenerateId() => id = Guid.NewGuid().ToString();

    public object CaptureState()
    {
        var state = new Dictionary<string, object>();
        foreach (var saveable in GetComponents<ISaveable>())
        {
            state[saveable.GetType().ToString()] = saveable.CaptureState();
        }
        return state;
    }

    public void RestoreState(object state)
    {
        var stateDictionary = (Dictionary<string, object>)state;
        foreach (var saveable in GetComponents<ISaveable>())
        {
            string typeName = saveable.GetType().ToString();
            if(stateDictionary.TryGetValue(typeName, out object savedState))
            {
                saveable.RestoreState(savedState);
            }
        }
    }
        
}
