using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Frame : MonoBehaviour
{

    [SerializeField] GameObject YellowFrame;
    [SerializeField] GameObject BlueFrame;
    [SerializeField] GameObject Item;

    void Update(){
        if(Item.GetComponent<Item_Drag>().nowSprite == null || Item.GetComponent<Item_Drag>().nowSprite.name == "Item_0000")
            BlueFrame.SetActive (false);
        else
            BlueFrame.SetActive (true);
    }

}
