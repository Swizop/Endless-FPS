
using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;

public class SaveLoading: MonoBehaviour
{
    public string SavePath => $"{Application.persistentDataPath}/save.txt";

    //[ContextMenu("Save")]
    public void Save()
    {
        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
    }

    //[ContextMenu("Load")]
    public void Load()
    {
            var state = LoadFile();
            RestoreState(state);
    }

  
    public void SaveFile(object state)
    {
            using(var stream = File.Open(SavePath,FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
    }

    Dictionary<string, object> LoadFile()
    {
            if(!File.Exists(SavePath))
            {
                Debug.Log("No save file found");
                return new Dictionary<string, object>();
            }
            using(FileStream stream = File.Open(SavePath, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
    }

    public void CaptureState(Dictionary<string, object> state)
    {
            foreach(var saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.Id] = saveable.CaptureState();
            }
    }

    public void RestoreState(Dictionary<string, object> state)
    {
        foreach(var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if(state.TryGetValue(saveable.Id, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
}
