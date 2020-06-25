using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Server;
using UnityEngine;
using VowelAServer.Shared.Data;
using VowelAServer.Shared.Data.Multiplayer;

public class SceneManager : MonoBehaviour
{
    public LayerMask InteractLayer;
    public static SceneManager Instance;
    public static Dictionary<string, Container> ObjectsTree = new Dictionary<string, Container>();

    private const string RootName = "Root";
    private GameObject root;

    void Awake()
    {
        Instance = this;
        // Pre-Init Scene
        root = new GameObject(RootName);

        ConnectionManager.ClientEventHandler += NetEventPoll_ServerEventHandler;
    }

    private SceneData DeserializeSceneData(ENet.Event netEvent) {
        var protocol = new Protocol();
        
        var readBuffer = new byte[netEvent.Packet.Length];
        netEvent.Packet.CopyTo(readBuffer);

        protocol.Deserialize(readBuffer, out var code, out string sceneFile);
        var sceneData = JsonConvert.DeserializeObject<SceneData>(sceneFile);
        return sceneData;
    }

    private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId){
        var netEvent = (ENet.Event) sender;
        if (packetId == PacketId.ObjectChangesEvent) {
            var sceneData = DeserializeSceneData(netEvent);
            if (sceneData != null) UpdateScene(sceneData);
        } else if (packetId == PacketId.AreaResponse) {
            var sceneData = DeserializeSceneData(netEvent);
            // Destroy everything on currect area
            if (sceneData != null) UpdateScene(sceneData);
        }
    }

    public void UpdateScene(SceneData sceneData) {
        foreach (var added in sceneData.Added) {
            // Adding new stuff
            var newGameObject = new GameObject(added.ContainerName);
            newGameObject.transform.position = new Vector3((float)added.Position.X, (float)added.Position.Y, (float)added.Position.Z);
            newGameObject.transform.localScale = new Vector3((float)added.Size.X, (float)added.Size.Y, (float)added.Size.Z);
            newGameObject.name = added.ContainerName;

            var container = newGameObject.AddComponent<Container>();
            container.SetData(added);
            ObjectsTree.Add(added.Id, container);
            if (ObjectsTree.TryGetValue(added.ParentId, out var parentContainer)){
                newGameObject.transform.SetParent(parentContainer.transform);  
            } else {
                newGameObject.transform.SetParent(root.transform);
            }
        }

        foreach (var removed in sceneData.Removed) {
            // Removing guids which are not using
        }

        foreach (var changed in sceneData.Changed) {
            // Changing objects which are not using
            if (ObjectsTree.TryGetValue(changed.Id, out var container)){
                container.transform.position = new Vector3((float)changed.Position.X, (float)changed.Position.Y, (float)changed.Position.Z);
                container.transform.localScale = new Vector3((float)changed.Size.X, (float)changed.Size.Y, (float)changed.Size.Z);
                container.gameObject.name = changed.ContainerName;
            }
        }
    }
}
