using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OpenEvent : MonoBehaviour
{
    public bool isMoved = false;
    private GameManager_Event GM_E;
    private GameManager_MapManage GM_M;
    private GameManager_ItemManage GM_I;

    void Start(){
        GM_E = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_Event>();
        GM_M = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MapManage>();
        GM_I = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_ItemManage>();
    }

    public void openEvent(){
        if(isMoved == true){
            Room thisRoom = GM_M.GetRoomDataList().First(room => room.hierarchy == GM_M.currentHierarchy && room.column == GM_M.currentLocale);
            if(thisRoom.roomEvent != 0){
                GM_E.StartEvent(thisRoom.roomEvent);
            }
            if(thisRoom.needKey == true){
                GM_I.DeleteItem( GM_I.GetItemDataList().First(item => item.getID() == 201001 && item.getSortNo() < 100).getSortNo() );
            }
            isMoved = false;
        }
    }

}
