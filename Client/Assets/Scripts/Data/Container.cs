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
        if (!string.IsNullOrEmpty(this.data.ClientLuaCode)) {
            ContainerScript = new RealTimeCompiler(this);
            ContainerScript.CompileData(this.data.ClientLuaCode);
        }
    }

    public ContainerData GetData() => data;
}
