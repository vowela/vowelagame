using System.Collections;
using System.Collections.Generic;
using ENet;
using Server;
using UnityEngine;
using VowelAServer.Shared.Data.Multiplayer;

public class AuthController : MonoBehaviour
{
    public static bool Authorized;

    // Start is called before the first frame update
    void Start()
    {
        ConnectionManager.ClientEventHandler += NetEventPoll_ServerEventHandler;
    }

    private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId){
        var netEvent = (ENet.Event) sender;
        if (packetId == PacketId.LoginEvent) {
        }
    }
}
