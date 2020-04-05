using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using VowelAServer.Shared.Data;
using System.IO;

public class RealTimeCompiler
{
    [HideInInspector] public Script ScriptObject;

    [HideInInspector] public Container Container;

    public RealTimeCompiler(Container container) { Container = container; }
    public RealTimeCompiler(string code) { CompileData(code); }

    private void Start() {
        if (ScriptObject != null) CallFunction("OnStart");
    }

    private void Update() {
        if (ScriptObject != null) CallFunction("OnUpdate");
    }

    private void FixedUpdate() {
        if (ScriptObject != null) CallFunction("OnFixedUpdate");
    }

    public DynValue CallFunction(string functionName) {
        var functionTable = GetData(functionName);
        if (functionTable != null) {
            var result = ScriptObject.Call(functionTable);
            return result;
        }
        return null;
    }

    public object GetData(string data) {
        var functionTable = ScriptObject.Globals[data];
        return functionTable;
    }

    public void CompileData(string luaCode) {
        ScriptObject = new Script(CoreModules.Preset_SoftSandbox);
        Mapper.Instance.InitMapping(this);
        ScriptObject.DoString(luaCode);
        Start();
    }
}
