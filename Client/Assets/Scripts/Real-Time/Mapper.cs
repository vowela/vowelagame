using System;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;
using VowelAServer.Shared.Data.Math;

public class Player {
    public static GameObject PlayerObject;
}
public class PlayerController {
    public static Vector GetPosition() {
        if (Player.PlayerObject == null) return new Vector();
        else {
            var pos = Player.PlayerObject.transform.position;
            return new Vector(pos.x, pos.y, pos.y);
        }
    }
}

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

    private void EditLogic() {

    }

    private void CreateObject(Vector playerPosition) {
        var newObject = new GameObject();
        newObject.transform.position = new Vector3((float)playerPosition.X, (float)playerPosition.Y, (float)playerPosition.Z) + Vector3.forward;
    }
    
    public void InitMapping(Script scriptObject) {
        UserData.RegisterType<Vector>();
        UserData.RegisterType<PlayerController>();
        scriptObject.Globals["Player"]  = typeof(PlayerController);

        scriptObject.Globals["CreateObject"]  = (Action<Vector>) CreateObject;
        scriptObject.Globals["EditLogic"]     = (Action) EditLogic;
    }
}
