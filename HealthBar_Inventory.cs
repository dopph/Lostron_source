using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar_Inventory : HealthBar
{

    private GameManager_MainScene GM;
    [SerializeField] int num;

    public override void setParent(){
        GM = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MainScene>();
        this.Parent = GM.Players[num].GetComponent<Character>();
        if(Parent.isDown == true)this.transform.gameObject.SetActive(false);
        Debug.Log(Parent);
    }

}
