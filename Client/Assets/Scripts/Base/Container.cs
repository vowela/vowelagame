using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VowelAServer.Shared.Data;

public class Container : MonoBehaviour
{
    private ContainerData data;

    public void SetData(ContainerData data) {
        this.data = data;
    }
}
