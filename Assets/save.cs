using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mono.Cecil.Cil;
using UnityEngine;

public class SaveManager
{
    public T load<T> (string path,bool willDo = true) where T:SavableObject
    {
        string Text = System.IO.File.ReadAllText(path);
        T savableObject = JsonUtility.FromJson<T>(Text);
        savableObject.loadObject(willDo);
        return savableObject;
    }

    public void save(SavableObject obj, string path,bool willDo = true)
    {
        obj.saveObject(willDo);
        string characterInventoryData = JsonUtility.ToJson(obj);
        Debug.Log(path);
        System.IO.File.WriteAllText(path, characterInventoryData);
    }
}

public class SavableObject
{
    public virtual void loadObject(bool willLoad = true)
    {

    }

    public virtual void saveObject(bool willSave = true)
    {
        
    }
}


