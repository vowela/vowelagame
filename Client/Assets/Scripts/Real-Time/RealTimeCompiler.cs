using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using VowelAServer.Shared.Data;

public class RealTimeCompiler : MonoBehaviour
{
    static void CompileData(ContainerObject container) {
        Debug.Log(container.LuaCode);
    }
}
