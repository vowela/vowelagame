using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VowelAServer.Shared.Data;

public class SceneManager : MonoBehaviour
{
    public static Dictionary<string, Container> ObjectsTree = new Dictionary<string, Container>();

    private const string RootName = "Root";
    private GameObject root;

    void Awake() {
        // Pre-Init Scene
        root = new GameObject(RootName);

    }

    public void CompileScene(SceneData sceneData) {
        if (sceneData == null) return;
        
        foreach (var added in sceneData.Added) {
            // Adding new stuff
            var newGameObject = new GameObject(added.ContainerName);
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
    }
}
