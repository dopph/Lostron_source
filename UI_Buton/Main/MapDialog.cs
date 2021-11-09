using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MapDialog : MonoBehaviour
{
    private GameManager_MapManage GM_M;
    private GameManager_ItemManage GM_I;

    public List<Room> Rooms;
    [SerializeField] GameObject hierarchyText;

    private bool hasKey = false;

    void Start()
    {

        GM_M = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MapManage>();
        GM_I = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_ItemManage>();

        Rooms = GM_M.GetRoomDataList();
        hierarchyText.GetComponent<Text>().text = "B-"+((GM_M.currentHierarchy+1).ToString());

        List<Item> Items = GM_I.GetItemDataList();
        hasKey = Items.Any(item => item.getID() == 201001 && item.getSortNo() < 100);

        //現在値を表示
        transform.Find( "RoomSet_01/Button_Room_0"+((GM_M.currentLocale+1).ToString())+"/Base_Player" ).gameObject.SetActive(true);
        transform.Find( "RoomSet_01/Button_Room_0"+((GM_M.currentLocale+1).ToString())+"/Base_01" ).gameObject.SetActive(false);
        if(Rooms.FirstOrDefault(room => room.column == GM_M.currentLocale && room.hierarchy == GM_M.currentHierarchy).straightBranch == true){
            if(hasKey || Rooms.FirstOrDefault(room => room.column == GM_M.currentLocale && room.hierarchy == GM_M.currentHierarchy+1).needKey == false){
                transform.Find( "RoomSet_02/Button_Room_0"+((GM_M.currentLocale+1).ToString())+"/Base_01" ).gameObject.SetActive(false);
                transform.Find( "RoomSet_02/Button_Room_0"+((GM_M.currentLocale+1).ToString()) ).GetComponent<Button>().enabled = true;
            }
        }
        if(Rooms.FirstOrDefault(room => room.column == GM_M.currentLocale && room.hierarchy == GM_M.currentHierarchy).leftBranch == true){
            if(hasKey || Rooms.FirstOrDefault(room => room.column == GM_M.currentLocale-1 && room.hierarchy == GM_M.currentHierarchy+1).needKey == false){
                transform.Find( "RoomSet_02/Button_Room_0"+((GM_M.currentLocale).ToString())+"/Base_01" ).gameObject.SetActive(false);
                transform.Find( "RoomSet_02/Button_Room_0"+((GM_M.currentLocale).ToString())).GetComponent<Button>().enabled = true;
            }
        }
        if(Rooms.FirstOrDefault(room => room.column == GM_M.currentLocale && room.hierarchy == GM_M.currentHierarchy).rightBranch == true){
            if(hasKey || Rooms.FirstOrDefault(room => room.column == GM_M.currentLocale+1 && room.hierarchy == GM_M.currentHierarchy+1).needKey == false){
                transform.Find( "RoomSet_02/Button_Room_0"+((GM_M.currentLocale+2).ToString())+"/Base_01" ).gameObject.SetActive(false);
                transform.Find( "RoomSet_02/Button_Room_0"+((GM_M.currentLocale+2).ToString())).GetComponent<Button>().enabled = true;
            }
        }

        for(int h=1; h<=4; h++){
            for(int c=1; c<=5; c++){

                //イベントと鍵マーク表示
                if(h <= 3 && Rooms.Count(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 1) == 1){

                    transform.Find("RoomSet_0"+((h).ToString())+"/Button_Room_0"+((c).ToString())).gameObject.SetActive(true);

                    if(Rooms.FirstOrDefault(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 1).needKey == true){
                        transform.Find("RoomSet_0"+((h).ToString())+"/Button_Room_0"+((c).ToString())+"/Line_Icon").gameObject.SetActive(true);
                        transform.Find("RoomSet_0"+((h).ToString())+"/Button_Room_0"+((c).ToString())+"/Line_Icon/IconSet/Icon_Lock").gameObject.SetActive(true);
                    }

                    if(Rooms.FirstOrDefault(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 1).roomEvent > 0 && Rooms.FirstOrDefault(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 1).needKey == false){
                        transform.Find("RoomSet_0"+((h).ToString())+"/Button_Room_0"+((c).ToString())+"/Line_Icon").gameObject.SetActive(true);
                        transform.Find("RoomSet_0"+((h).ToString())+"/Button_Room_0"+((c).ToString())+"/Line_Icon/IconSet/Icon_Enemy").gameObject.SetActive(true);
                    }

                    if(Rooms.FirstOrDefault(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 1).risk == 1 || Rooms.FirstOrDefault(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 1).risk == -1){
                        transform.Find("RoomSet_0"+((h).ToString())+"/Button_Room_0"+((c).ToString())+"/Base_Yellow").gameObject.SetActive(true);
                    }else if(Rooms.FirstOrDefault(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 1).risk == 2 || Rooms.FirstOrDefault(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 1).risk == -2){
                        transform.Find("RoomSet_0"+((h).ToString())+"/Button_Room_0"+((c).ToString())+"/Base_Red").gameObject.SetActive(true);
                    }else if(Rooms.FirstOrDefault(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 1).risk == 3){
                        transform.Find("RoomSet_0"+((h).ToString())+"/Button_Room_0"+((c).ToString())+"/Base_Purple").gameObject.SetActive(true);
                    }
                }

                //次エリアのライン表示
                if(Rooms.Count(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 2) == 1){

                    if(Rooms.FirstOrDefault(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 2).straightBranch == true){
                        transform.Find("LinePanelSet/Mask/Line_Set_0"+((h).ToString())+"/LineObject_0"+(c.ToString())+"/Line_Top_To_Bot").gameObject.SetActive(true);
                    }
                    if(Rooms.FirstOrDefault(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 2).leftBranch == true){
                        transform.Find("LinePanelSet/Mask/Line_Set_0"+((h).ToString())+"/LineObject_0"+(c.ToString())+"/Line_Top_To_Left").gameObject.SetActive(true);
                        transform.Find("LinePanelSet/Mask/Line_Set_0"+((h).ToString())+"/LineObject_0"+((c-1).ToString())+"/Line_Right_To_Bot").gameObject.SetActive(true);
                    }
                    if(Rooms.FirstOrDefault(room => room.column == c-1 && room.hierarchy == GM_M.currentHierarchy + h - 2).rightBranch == true){
                        transform.Find("LinePanelSet/Mask/Line_Set_0"+((h).ToString())+"/LineObject_0"+(c.ToString())+"/Line_Top_To_Right").gameObject.SetActive(true);
                        transform.Find("LinePanelSet/Mask/Line_Set_0"+((h).ToString())+"/LineObject_0"+((c+1).ToString())+"/Line_Left_To_Bot").gameObject.SetActive(true);
                    }
                }

            }
        }
    }

}
