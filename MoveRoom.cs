using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveRoom : MonoBehaviour, IPointerEnterHandler
{

    public GameObject MoveDialog;

    [SerializeField] GameObject CheckDialog;
    [SerializeField] GameObject WipeAnimation;
    [SerializeField] GameObject WipeAnimation_Key;
    [SerializeField] int column;
    [SerializeField] int hierarchy;
    [SerializeField] Animator AnimObject;

    private GameManager_MainScene GM;
    private GameManager_MapManage GM_M;
    private GameManager_Event GM_E;
    private GameManager_TimeScale GM_T;
    private GameManager_ItemManage GM_I;
    private List<Room> Rooms = new List<Room>();
    private GameObject window;

    public void Start()
    {

        GM   = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MainScene>();
        GM_M = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MapManage>();
        GM_E = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_Event>();
        GM_T = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_TimeScale>();
        GM_I = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_ItemManage>();
        Rooms = GM_M.GetRoomDataList();
        
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        transform.parent.parent.Find("Text").gameObject.GetComponent<Text>().text = GM_E.getEventList().First(ev => ev.ID == (GM_M.GetRoomDataList().FirstOrDefault(room => room.column == column && room.hierarchy == GM_M.currentHierarchy + hierarchy).roomEvent) ).RoomExplain;
    }

    public void startMove(){
        StartCoroutine("CStartMove");
    }

    private IEnumerator CStartMove(){

        if(transform.Find("Base_01").gameObject.activeSelf == false){

            int thisHierarchy = GM_M.currentHierarchy + hierarchy;

            if(GM_M.GetRoomDataList().FirstOrDefault(room => room.column == column && room.hierarchy == thisHierarchy).needKey == true){

                window = Instantiate(CheckDialog);
                window.transform.SetParent(this.transform.parent.parent.parent.parent.parent.parent.parent);
                window.GetComponent<CommonDialog>().SetTitleText("確認");
                window.GetComponent<CommonDialog>().SetMainText("カードキーを使いますか？");

                while(window.GetComponent<CommonDialog>().GetValue() == -1){
                    yield return null;
                }
                Debug.Log(window.GetComponent<CommonDialog>().GetValue());
                if(window.GetComponent<CommonDialog>().GetValue() == 0){
                    yield break;
                }

            }

            AnimObject.SetTrigger("Click_Close");
            GM_M.currentLocale = column;
            GM_M.currentHierarchy += hierarchy;
            if(GM_M.GetRoomDataList().FirstOrDefault(room => room.column == GM_M.currentLocale && room.hierarchy == GM_M.currentHierarchy).needKey == false){
                window = Instantiate(WipeAnimation);
            }else{
                window = Instantiate(WipeAnimation_Key);
            }
            window.transform.SetParent(this.transform.parent.parent.parent.parent.parent.parent.parent);
            OpenEvent oe = window.transform.Find("Canvas/AnimObject").GetComponent<OpenEvent>(); // Open Event
            oe.isMoved = true;

            GM_I.ClearLoot();

            List<GameObject> ClearList = GM.AllObject.Where(obj => obj.GetComponent<Character>().isDown == true).ToList();
            foreach(GameObject obj in ClearList){
                GM.AllObject.Remove(obj);
                DestroyImmediate(obj);
            }
            GM_M.BG_Set();

        }
        yield return null;
    }

    private IEnumerator CheckUseKey(){
        return null;
    }

    public void ClearDead(GameObject obj){

    }


}
