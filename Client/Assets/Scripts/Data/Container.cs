using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;
using VowelAServer.Shared.Data;

public class Container : MonoBehaviour
{
    [HideInInspector]
    public RealTimeCompiler ContainerScript;
    private ContainerData data;

    public void SetData(ContainerData data) {
        this.data = data;
        if (!string.IsNullOrEmpty(this.data.LuaCode)) {
            ContainerScript = new RealTimeCompiler();
            ContainerScript.CompileData(this.data.LuaCode);
        }
    }

    public ContainerData GetData() {
        return this.data;
    }
}
