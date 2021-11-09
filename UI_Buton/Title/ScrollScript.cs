using EnriqueDavid.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    public int Number;
    [SerializeField] GameObject[] Characters;

    public void OnIndexChanged(IndexChangedEventData data)
    {
        StartItem.StartCharaID[Number] = Characters[data.index];
        Debug.Log(StartItem.StartCharaID[Number]);
    }
}
