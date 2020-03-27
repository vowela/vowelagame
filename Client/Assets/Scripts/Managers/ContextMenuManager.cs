using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VowelAServer.Shared.Data;

public class ContextMenuManager : MonoBehaviour
{
    public GameObject ContextMenuPrefab;

    private GameObject contextMenuInstance;

    private void Update() {
        if (Input.GetMouseButtonUp(0)) {
            if (contextMenuInstance != null)
                contextMenuInstance.GetComponent<ContextMenu>().HideMenu();
        } else if (Input.GetMouseButtonDown(1)) {
            if (contextMenuInstance == null)
                contextMenuInstance = Instantiate(ContextMenuPrefab, Vector3.zero, Quaternion.identity, UIManager.Instance.CanvasUI.transform);
            var contextMenu = contextMenuInstance.GetComponent<ContextMenu>();
            contextMenu.SetPosition();

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100.0f)) {
                // Object scripting
            } else {
                // World space scripting
                // Call Server to fetch context menu data
                var menuJson = TestWorldContextMenu();

                // Creating context menu based on json data
                var menuData = JsonUtility.FromJson<MenuData>(menuJson);
                var contextMenuData = new ContextMenuData();
                foreach (var menuButton in menuData.ButtonData) {
                    contextMenuData.ButtonData.Add(new ContextMenuButtonData {
                            ButtonText = menuButton.ButtonText,
                            LuaCompiler = new RealTimeCompiler(menuButton.LuaCode)
                        });
                }
                contextMenu.InitContextMenu(contextMenuData);
            }
        }
    }

    private string TestWorldContextMenu() {
        var outJson = "";
        var testingJson = new MenuData() {
            ButtonData = new List<MenuButtonData>() {
                new MenuButtonData {
                    ButtonText = "Новый объект",
                    LuaCode = "function OnClick() CreateGameObject('New Object Init') end"
                }
            }
        };

        outJson = JsonUtility.ToJson(testingJson);
        return outJson;
    }
}
