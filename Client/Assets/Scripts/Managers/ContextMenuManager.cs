﻿using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Server;
using UnityEngine;
using VowelAServer.Shared.Data;
using VowelAServer.Shared.Data.Multiplayer;
using VowelAGame;
using ContextMenu = VowelAGame.ContextMenu;

public class ContextMenuManager : MonoBehaviour
{
    public static ContextMenuManager Instance;

    public Container HitContainer;
    public LayerMask ObjectsMask;
    public GameObject ContextMenuPrefab;

    private GameObject contextMenuInstance;
    private ContextMenu contextMenu;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start() {
        ConnectionManager.ClientEventHandler += NetEventPoll_ServerEventHandler;
    }

    private void Update() {
        if (Input.GetMouseButtonUp(0)) {
            if (contextMenuInstance != null)
            {
                contextMenuInstance.GetComponent<ContextMenu>().HideMenu();
                HitContainer = null;
            }
        } else if (Input.GetMouseButtonDown(1)) {
            if (!ConnectionManager.IsConnected) return;

            if (contextMenuInstance == null)
                contextMenuInstance = Instantiate(ContextMenuPrefab, Vector3.zero, Quaternion.identity, UIManager.Instance.CanvasUI.transform);
            contextMenu = contextMenuInstance.GetComponent<ContextMenu>();
            contextMenu.SetPosition();

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            byte[] data;
            if (Physics.Raycast(ray, out var hit, 100.0f, ObjectsMask)) {
                // Object scripting
                data = Protocol.SerializeData((byte)PacketId.MenuRequest, "object");
                HitContainer = hit.transform.GetComponentInParent<Container>();
            } else {
                // World space scripting
                // Call Server to fetch context menu data
                data = Protocol.SerializeData((byte)PacketId.MenuRequest, "context");
                HitContainer = null;
            }
            NetController.SendData(data);
        }
    }

    private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId){
        var netEvent = (ENet.Event) sender;
        if (packetId == PacketId.MenuResponse) {
            var protocol = new Protocol();
            
            var readBuffer = new byte[netEvent.Packet.Length];
            netEvent.Packet.CopyTo(readBuffer);

            protocol.Deserialize(readBuffer, out var code, out string contextData);

            // Creating context menu based on json data
            var menuData = JsonConvert.DeserializeObject<MenuData>(contextData);
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
