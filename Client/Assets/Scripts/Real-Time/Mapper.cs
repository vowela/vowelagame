using System;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using Newtonsoft.Json;
using Server;
using UnityEngine;
using VowelAServer.Shared.Data;
using VowelAServer.Shared.Data.Math;
using VowelAServer.Shared.Data.Multiplayer;

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
        byte[] data;
        var newObject = new ContainerData {
            Position = playerPosition,
            Size = new Vector(1, 1, 1)
        };
        data = Protocol.SerializeData((byte)PacketId.ObjectChangesRequest, JsonConvert.SerializeObject(newObject));
        NetController.SendData(data);
    }
    
    public void InitMapping(Script scriptObject) {
        UserData.RegisterType<Vector>();
        UserData.RegisterType<PlayerController>();
        scriptObject.Globals["Player"]  = typeof(PlayerController);

        scriptObject.Globals["CreateObject"]  = (Action<Vector>) CreateObject;
        scriptObject.Globals["EditLogic"]     = (Action) EditLogic;
        scriptObject.Globals["Log"]           = (Action<string>) ((str) => Debug.Log(str));
    }
}
