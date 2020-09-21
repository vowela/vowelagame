using System.Collections.Generic;
using UnityEngine;
using VowelAServer.Shared.Models.Db;
using VowelAServer.Shared.Models.Multiplayer;

using RPC = VowelAServer.Shared.Models.RPC;

public class DbController: StaticNetworkComponent
{
    public delegate void CollectionsHandler(IEnumerable<string> collections);
    public static event CollectionsHandler CollectionsNotify;

    public delegate void CollectionItemsHandler(IEnumerable<Row> rows);
    public static event CollectionItemsHandler CollectionItemsNotify;
        
    [RPC] public static void GetCollectionNames(IEnumerable<string> collections)
    {
        CollectionsNotify?.Invoke(collections);
    }

    [RPC]
    public static void GetCollectionItems(IEnumerable<Row> rows)
    {
        CollectionItemsNotify?.Invoke(rows);
    }

    public static void RequestCollectionNames() => RPC("DbController", "GetCollectionNames");

    public static void RequestCollectionItems(string collection) => RPC("DbController", "GetCollectionItems", collection);
}