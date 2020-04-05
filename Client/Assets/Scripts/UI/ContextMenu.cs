using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenuButtonData {
    public string ButtonText;
    public RealTimeCompiler LuaCompiler;
}

public class ContextMenuData {
    public List<ContextMenuButtonData> ButtonData = new List<ContextMenuButtonData>();
}

public class ContextMenu : MonoBehaviour
{
    public GameObject ButtonTemplate;
    private List<GameObject> buttonInstances = new List<GameObject>();

    public void InitContextMenu(ContextMenuData data) {
        foreach (var buttonData in data.ButtonData) {
            var newButton = GameObject.Instantiate(ButtonTemplate);
            newButton.transform.SetParent(transform);
            var buttonScript = newButton.GetComponent<Button>();
            buttonScript.onClick.AddListener(() => { buttonData.LuaCompiler.CallFunction("OnClick"); });
            buttonScript.GetComponentInChildren<Text>().text = buttonData.ButtonText;
            newButton.SetActive(true);
            buttonInstances.Add(newButton);
        }
    }

    public void HideMenu() {
        foreach (var button in buttonInstances)
            GameObject.Destroy(button);
        gameObject.SetActive(false);
    }

    public void SetPosition()
    {
        HideMenu();
        transform.position = Input.mousePosition;
        gameObject.SetActive(true);
    }
}
