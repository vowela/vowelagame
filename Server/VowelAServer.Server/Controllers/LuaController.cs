
using ENet;
using VowelAServer.Shared.Data.Multiplayer;

namespace VowelAServer.Server.Controllers
{
    public class LuaController
    {
        public LuaController()
        {
            NetEventPoll.ServerEventHandler += NetEventPoll_ServerEventHandler;
        }

        private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId)
        {
            var senderEvent = (Event)sender;
            if (packetId == PacketId.LuaRequest)
            {

            }
        }
    }
}
