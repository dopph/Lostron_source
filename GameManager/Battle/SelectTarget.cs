using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTarget : MonoBehaviour
{
    private GameManager_MainScene GM;
    [SerializeField] CommandWindow Parent;
    [SerializeField] int Number;
    private GameObject TargetObj;

    void Start(){

        GM = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MainScene>();

        if(Number == 0)TargetObj = GM.Players[0];
        if(Number == 1)TargetObj = GM.Players[1];
        if(Number == 2)TargetObj = GM.Players[2];
        if(Number == 3)TargetObj = GM.Enemies[0];
        if(Number == 4)TargetObj = GM.Enemies[1];
        if(Number == 5)TargetObj = GM.Enemies[2];

        if(TargetObj.GetComponent<Character>().isDown == true)transform.gameObject.SetActive(false);
    }

    public void DecideTarget(){
        if(TargetObj.GetComponent<Character>().isDown == false){
            if(Number >= 3){
                Parent.TargetEnemy = TargetObj;
            }else{
                Parent.TargetAlly = TargetObj;
            }
        }
        Parent.HighlightUpdate();
    }

}
