using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [HideInInspector]
    public Canvas CanvasUI;

    private void Awake() {
        Instance = this;
        CanvasUI = FindObjectOfType<Canvas>();
    }
}
