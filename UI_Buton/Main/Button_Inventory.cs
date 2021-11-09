using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Inventory : MonoBehaviour
{
    [SerializeField] GameManager_MainScene GM;
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject window;

    public void Start()
    {
        GM = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MainScene>();
    }

    public void OnClickStartButton()
    {
        if(GM.Phase == GameManager_MainScene.Phases.IDLE){
            window = Instantiate(Inventory);
            window.transform.SetParent(this.transform.parent.parent.parent.parent);
        }
    }

}
