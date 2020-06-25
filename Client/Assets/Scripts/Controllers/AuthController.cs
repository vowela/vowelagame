using System.Collections;
using System.Collections.Generic;
using ENet;
using Newtonsoft.Json;
using Server;
using UnityEngine;
using VowelAServer.Shared.Data.Multiplayer;
using VowelAServer.Shared.Models.Dtos;

public class AuthController : MonoBehaviour
{
    private string login;
    private string password;

    void Start()
    {
        ConnectionManager.ClientEventHandler += NetEventPoll_ServerEventHandler;
    }

    public void OnLoginChange(string login) => this.login = login;

    public void OnPasswordChange(string password) => this.password = password;

    public void Login()
    {
        var user = new UserDto()
        {
            Login = login,
            Password = password,
        };

        var serializedUser = JsonConvert.SerializeObject(user);

        //var data = Protocol.SerializeData((byte)PacketId.LoginRequest, serializedUser);
        //NetController.SendData(data);
    }

    private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId){
        var netEvent = (ENet.Event) sender;
        /*if (packetId == PacketId.ObjectChangesEvent) {
            var readBuffer = new byte[netEvent.Packet.Length];
            netEvent.Packet.CopyTo(readBuffer);

            var protocol = new Protocol();
            protocol.Deserialize(readBuffer, out var code, out byte isLoggedIn);

            if (isLoggedIn == 1)
            {
                Debug.Log("Login successful");
            } else
            {
                Debug.Log("Login failed");
            }
        }*/
    }
}
