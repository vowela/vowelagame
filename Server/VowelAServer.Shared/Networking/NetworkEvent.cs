namespace VowelAServer.Shared.Networking
{
    public enum NetworkEvent
    {
        // Network Event Types
        None,
        ClientLogin,        // After connect, Server->Client send initial login data, initialize client network object ID for later RPCs
        ClientUpdate,       // sending a list of relevant objects to the client
        RPC,                // call an RPC on the given network object
        RPCResponse,        // the response value to a called RPC,
        DisconnectReason    // disconnect reason, gives a chance to send a disconnect reason to peer before disconnect
    }
}