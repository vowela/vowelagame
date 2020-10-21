using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VowelAServer.Shared.Models.Db;

public class DbUI : MonoBehaviour
{
    public Transform CollectionsList;
    public Transform TabelPanelContent;

    public GameObject HeaderCell;
    public GameObject BodyCell;
    public GameObject TableRow;
    
    // Start is called before the first frame update
    void Start()
    {
        DbController.CollectionsNotify += RenderCollections;
        DbController.CollectionItemsNotify += RenderCollectionItems;
        
        DbController.RequestCollectionNames();
    }

    private void RenderCollections(IEnumerable<string> collectionNames)
    {
        foreach (var collectionName in collectionNames)
        {
            var button = DefaultControls.CreateButton(new DefaultControls.Resources());

            button.GetComponentInChildren<Text>().text = collectionName;
            button.GetComponent<Button>().onClick.AddListener(delegate { DbController.RequestCollectionItems(collectionName); });
            
            button.transform.SetParent(CollectionsList, false);
        }
    }

    private void RenderCollectionItems(IEnumerable<Row> rows)
    {
        ClearTable();
        
        var firstRow = rows.First();
        if (firstRow == null) return;

        RenderTableHeader(firstRow);
        RenderBody(rows);
    }

    private void RenderTableHeader(Row header)
    {
        var headerRow = Instantiate(TableRow);

        foreach (var item in header.Items)
        {
            RenderCell(HeaderCell, item.Key, headerRow.transform);
            
            headerRow.transform.SetParent(TabelPanelContent, false);
        }
    }

    private void RenderBody(IEnumerable<Row> rows)
    {
        foreach (var row in rows)
        {
            var tableRow = Instantiate(TableRow);
            
            foreach (var item in row.Items)
            {
                RenderCell(BodyCell, item.Value, tableRow.transform);
            
                tableRow.transform.SetParent(TabelPanelContent, false);
            }
        }
    }

    private void ClearTable()
    {
        for (var i = TabelPanelContent.childCount - 1; i >= 0; i--)
        {
            Destroy(TabelPanelContent.GetChild(i).gameObject);
        }
    }

    private void RenderCell(GameObject cellPrefab, String value, Transform parent)
    {
        var cell = Instantiate(cellPrefab);
                
        var cellText = cell.gameObject.GetComponent<TextMeshProUGUI>();
        cellText.SetText(value);
                
        cell.transform.SetParent(parent, false);
    }
}
