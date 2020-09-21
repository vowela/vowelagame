using VowelAServer.Db.DbParser;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Models;
using VowelAServer.Utilities.Network;

namespace VowelAServer.Server.Controllers
{
    public class DbController: StaticNetworkObject
    {
        [RPC] public static void GetCollectionNames(Player player)
        {
            var collections = DbViwer.GetCollectionNames();

            RPC(player.NetPeer, "DbController", "GetCollectionNames", collections);
        }

        [RPC] public static void GetCollectionItems(Player player, string collection)
        {
            var items = DbViwer.GetAllItemsInCollection(collection);

            RPC(player.NetPeer, "DbController", "GetCollectionItems", items);
        }
    }
}
