using System;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

public class Mapper
{
    private static Mapper MapperInstance;
    public static Mapper Instance{
        get {
            if (MapperInstance == null)
                MapperInstance = new Mapper();
            return MapperInstance;
        }
    }
    private void CreateObject(string objectName) {
        var newObject = new GameObject(objectName);
    }
    
    public void InitMapping(Script scriptObject) {
        scriptObject.Globals["CreateGameObject"] = (Action<string>) CreateObject;
    }
}
