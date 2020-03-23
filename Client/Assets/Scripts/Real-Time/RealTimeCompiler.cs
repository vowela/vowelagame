using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class RealTimeCompiler : MonoBehaviour
{
    
    public bool IsCompileScript;

    // Start is called before the first frame update
    void Start()
    {
    }

    double MoonSharpFactorial() {
        string script = @"
        --defines a factorial function
        function fact(n) 
            if (n == 0) then
                return 1
            else 
                return n*fact(n-1)
            end
        end
        
        print(fact(5))";

        var res = Script.RunString(script);
        return res.Number;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCompileScript) {
            Debug.Log("Compiled func: " + MoonSharpFactorial());
            IsCompileScript = false;
        }
    }
}
