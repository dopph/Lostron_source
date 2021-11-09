using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelect_CloseStoryWindow : MonoBehaviour
{
    [SerializeField] GameObject window;
    [SerializeField] GameObject BGM;
    public void StartItemSelect(){
        window.SetActive(true);
        BGM.SetActive(true);
    }
}
