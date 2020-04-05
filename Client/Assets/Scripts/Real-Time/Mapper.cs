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
            var pos = Player.PlayerObject.transform.position + Player.PlayerObject.transform.forward;
            return new Vector(pos.x, pos.y, pos.z);
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

    public void InitMapping(RealTimeCompiler compiler) {
        var scriptObject = compiler.ScriptObject;

        UserData.RegisterType<Vector>();
        scriptObject.Globals["Vector"]          = typeof(Vector);
        UserData.RegisterType<PlayerController>();
        scriptObject.Globals["Player"]          = typeof(PlayerController);
        UserData.RegisterType<ContainerData>();
        scriptObject.Globals["ContainerData"]   = typeof(ContainerData);

        if (compiler.Container != null)
            scriptObject.Globals["This"]        = compiler.Container.GetData();
        scriptObject.Globals["CreateObject"]    = (Action<Vector>) CreateObject;
        scriptObject.Globals["CreatePrimitive"] = (Action<uint>) ((uint primType) => CreatePrimitive(compiler.Container, primType));
        scriptObject.Globals["EditLogic"]       = (Action) EditLogic;
        scriptObject.Globals["Log"]             = (Action<string>) ((str) => Debug.Log(str));
    }

    private void EditLogic() {

    }

    private void CreatePrimitive(Container container, uint primType) {
        var newPrimitive = GameObject.CreatePrimitive((PrimitiveType)primType);
        newPrimitive.transform.SetParent(container.transform);
        newPrimitive.transform.localPosition = Vector3.zero;
    }

    private void CreateObject(Vector position) {
        byte[] data;
        var newObject = new ContainerData {
            Position = position,
            Size     = new Vector(1, 1, 1)
        };
        data = Protocol.SerializeData((byte)PacketId.ObjectChangesRequest, JsonConvert.SerializeObject(newObject));
        NetController.SendData(data);
    }
}
