using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using VowelAServer.Shared.Data;
using System.IO;

public class RealTimeCompiler
{
    private Script scriptObject;

    public RealTimeCompiler() {}
    public RealTimeCompiler(string code) { CompileData(code); }

    public DynValue CallFunction(string functionName) {
        var functionTable = GetData(functionName);
        if (functionTable != null) {
            var result = scriptObject.Call(functionTable);
            return result;
        }
        return null;
    }

    public object GetData(string data) {
        var functionTable = scriptObject.Globals[data];
        return functionTable;
    }

    public void CompileData(string luaCode) {
        scriptObject = new Script(CoreModules.Preset_SoftSandbox);
        Mapper.Instance.InitMapping(scriptObject);
        scriptObject.DoString(luaCode);
    }
}
