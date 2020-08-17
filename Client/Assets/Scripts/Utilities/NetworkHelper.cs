using System.Collections;
using System.Collections.Generic;
using ENet;
using UnityEngine;

public class NetworkHelper
{
    public static void SendData(byte[] buffer)
    {
        var packet = default(Packet);
        packet.Create(buffer);
        ConnectionManager.CurrentPeer.Send(0, ref packet);
    }
}
