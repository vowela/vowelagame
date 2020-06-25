using System;
using System.Collections;
using System.Collections.Generic;
using InGameCodeEditor;
using UnityEngine;

public class LogicEditorManager : MonoBehaviour
{
    public GameObject CodeEditorObject;
    public static LogicEditorManager Instance;

    private GameObject _codeEditorInstance;
    private CodeEditor _activeCodeEditorScript;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _codeEditorInstance = Instantiate(CodeEditorObject, UIManager.Instance.CanvasUI.transform);
        CloseEditor();
    }

    public void OpenEditor()
    {
        _codeEditorInstance.SetActive(true);
        _activeCodeEditorScript = _codeEditorInstance.GetComponent<CodeEditor>();
    }

    public void CloseEditor()
    {
        _codeEditorInstance.SetActive(false);
    }
    

    public void SetCode(string code)
    {
        _activeCodeEditorScript.Text = code;
    }
}
