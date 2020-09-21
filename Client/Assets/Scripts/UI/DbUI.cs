using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VowelAServer.Shared.Models.Db;

public class DbUI : MonoBehaviour
{
    public Transform CollectionsList;
    
    // Start is called before the first frame update
    void Start()
    {
        DbController.CollectionsNotify += RenderCollections;
        DbController.CollectionItemsNotify += RenderCollectionItems;
        
        DbController.RequestCollectionNames();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RenderCollections(IEnumerable<string> collectionNames)
    {
        foreach (var collectionName in collectionNames)
        {
            Debug.Log(collectionName);
            var button = DefaultControls.CreateButton(new DefaultControls.Resources());

            button.GetComponentInChildren<Text>().text = collectionName;
            button.GetComponent<Button>().onClick.AddListener(delegate { DbController.RequestCollectionItems(collectionName); });
            
            button.transform.SetParent(CollectionsList, false);
        }
    }

    private void RenderCollectionItems(IEnumerable<Row> rows)
    {
        // TODO: render table
    }
}
