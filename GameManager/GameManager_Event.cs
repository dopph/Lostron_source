using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public partial class GameManager_Event : MonoBehaviour{

    [SerializeField] Events EventDBsheet;
    [SerializeField] GameObject EventDialog;
    [SerializeField] GameManager_MainScene GM;
    [SerializeField] GameManager_MapManage GM_M;
    [SerializeField] GameManager_TimeScale GM_T;
    [SerializeField] GameManager_ItemManage GM_I; 
    [SerializeField] GameObject Inventory;
    [SerializeField] List<GameObject> PlayerData;
    [SerializeField] List<GameObject> EnemyData;

    private List<EventDB> EventDB = new List<EventDB>();
    public GameObject window;
    private EventDB e;
    private int LastEvent;
    private List<string> EventFlag = new List<string>();
    
    void Awake() {

        //各シートのデータを順に取り出す
        foreach (Events.Sheet EventSheet in EventDBsheet.sheets) {
            //各項目のデータを順に取り出す
            foreach (Events.Param EP in EventSheet.list) {
                if(EP.ID != 0){
                    EventDB.Add(new EventDB(EP.ID, EP.Risk, EP.RoomExplain, EP.Title, EP.Text, EP.selectA_Text, EP.selectA_JumpID, EP.selectA_Require, EP.selectB_Text, EP.selectB_JumpID, EP.selectB_Require, EP.selectC_Text, EP.selectC_JumpID, EP.selectC_Require));
                }
            }
        }
        
    }

    public List<EventDB> getEventList(){
        return EventDB;
    }

    public int getLastEvent(){
        return LastEvent;
    }
    public void setLastEvent(int Value){
        LastEvent = Value;
    }

    public void StartEvent(int ID){
        StartCoroutine(callWindow(ID));
    }

    IEnumerator callWindow(int ID){

        GameObject InventoryWindow;
        window = Instantiate(EventDialog);
        window.transform.SetParent(this.transform);
        e = EventDB.First(ev => ev.ID == ID);
        loadWindow(ID);

        while (window != null){

            if(window.GetComponent<CommonDialog>().GetValue() == 1){
                window.GetComponent<CommonDialog>().SetValue(-1);
                if(e.selectA_JumpID > 1){
                    window.transform.Find("Canvas/AnimObject").GetComponent<Animator>().SetTrigger("DialogChange");
                    yield return new WaitForSecondsRealtime(0.05f);
                    loadWindow(e.selectA_JumpID);
                }else if(e.selectA_JumpID == 1){
                    window.transform.Find("Canvas/AnimObject").GetComponent<Animator>().SetTrigger("Close");
                    yield return new WaitForSecondsRealtime(0.05f);
                    InventoryWindow = Instantiate(Inventory);
                    InventoryWindow.transform.SetParent(this.transform);

                }else{
                    window.transform.Find("Canvas/AnimObject").GetComponent<Animator>().SetTrigger("Close");
                }

            }else if(window.GetComponent<CommonDialog>().GetValue() == 2){
                window.GetComponent<CommonDialog>().SetValue(-1);
                if(e.selectB_JumpID > 1){
                    window.transform.Find("Canvas/AnimObject").GetComponent<Animator>().SetTrigger("DialogChange");
                    yield return new WaitForSecondsRealtime(0.05f);
                    loadWindow(e.selectB_JumpID);
                }else if(e.selectB_JumpID == 1){
                    window.transform.Find("Canvas/AnimObject").GetComponent<Animator>().SetTrigger("Close");
                    yield return new WaitForSecondsRealtime(0.05f);
                    InventoryWindow = Instantiate(Inventory);
                    InventoryWindow.transform.SetParent(this.transform);

                }else{
                    window.transform.Find("Canvas/AnimObject").GetComponent<Animator>().SetTrigger("Close");
                }

            }else if(window.GetComponent<CommonDialog>().GetValue() == 3){
                window.GetComponent<CommonDialog>().SetValue(-1);
                if(e.selectC_JumpID > 1){
                    window.transform.Find("Canvas/AnimObject").GetComponent<Animator>().SetTrigger("DialogChange");
                    yield return new WaitForSecondsRealtime(0.05f);
                    loadWindow(e.selectC_JumpID);
                }else if(e.selectC_JumpID == 1){
                    window.transform.Find("Canvas/AnimObject").GetComponent<Animator>().SetTrigger("Close");
                    yield return new WaitForSecondsRealtime(0.05f);
                    InventoryWindow = Instantiate(Inventory);
                    InventoryWindow.transform.SetParent(this.transform);

                }else{
                    window.transform.Find("Canvas/AnimObject").GetComponent<Animator>().SetTrigger("Close");
                }
            }
            yield return null;

        }
    }


    private void loadWindow(int ID){

        e = EventDB.First(ev => ev.ID == ID);

        setLastEvent(ID);

        if(e.Title == "%"){

            int RandomSum = 0;
            string[] PersentString = e.Text.Split(',');
            List<int> Persent = new List<int>();

            foreach (string Value in PersentString) {
                Persent.Add(int.Parse(Value));
            }

            foreach (int Bias in Persent) {
                RandomSum += Bias;
            }

            RandomSum = Random.Range(0, RandomSum)+1;
            int n = 0;
            
            foreach (int Bias in Persent) {
                RandomSum -= Bias;
                
                if(RandomSum <= 0){

                    if(n == 0){
                        loadWindow(e.selectA_JumpID);
                        return; 
                    }
                    if(n == 1){
                        loadWindow(e.selectB_JumpID);
                        return; 
                    }
                    if(n == 2){
                        loadWindow(e.selectC_JumpID);
                        return; 
                    }
                    
                }
                n++;
            }

        }
        loadEvents(ID);

        window.GetComponent<CommonDialog>().SetTitleText( e.Title );
        window.GetComponent<CommonDialog>().SetMainText( e.Text );

        window.GetComponent<CommonDialog>().SetBtn1Text( e.selectA_Text );
        
        if(e.selectA_Text == ""){
            window.GetComponent<CommonDialog>().Button1.SetActive(false);
        }else{
            window.GetComponent<CommonDialog>().Button1.SetActive(true);
        }
        if(e.selectA_Require != 0 && SelectRequire(e.selectA_Require) == false){
            window.GetComponent<CommonDialog>().Button1.SetActive(false);
        }

        window.GetComponent<CommonDialog>().SetBtn2Text( e.selectB_Text );
        if(e.selectB_Text == ""){
            window.GetComponent<CommonDialog>().Button2.SetActive(false);
        }else{
            window.GetComponent<CommonDialog>().Button2.SetActive(true);
        }
        if(e.selectB_Require != 0 && SelectRequire(e.selectB_Require) == false){
            window.GetComponent<CommonDialog>().Button2.SetActive(false);
        }

        window.GetComponent<CommonDialog>().SetBtn3Text( e.selectC_Text );
        if(e.selectC_Text == ""){
            window.GetComponent<CommonDialog>().Button3.SetActive(false);
        }else{
            window.GetComponent<CommonDialog>().Button3.SetActive(true);
        }
        if(e.selectC_Require != 0 && SelectRequire(e.selectC_Require) == false){
            window.GetComponent<CommonDialog>().Button3.SetActive(false);
        }
    }

}

public class EventDB{

    public int ID;
    public int Risk;
    public string RoomExplain;
    public string Title;
    public string Text;
    public string selectA_Text;
    public int selectA_JumpID;
    public int selectA_Require;
    public string selectB_Text;
    public int selectB_JumpID;
    public int selectB_Require;
    public string selectC_Text;
    public int selectC_JumpID;
    public int selectC_Require;

    public EventDB(int ID, int Risk, string RoomExplain, string Title, string Text, string selectA_Text, int selectA_JumpID, int selectA_Require, string selectB_Text, int selectB_JumpID, int selectB_Require, string selectC_Text, int selectC_JumpID, int selectC_Require){
        this.ID = ID;
        this.Risk = Risk;
        this.RoomExplain = RoomExplain;
        this.Title = Title;
        this.Text = Text;
        this.selectA_Text = selectA_Text;
        this.selectA_JumpID = selectA_JumpID;
        this.selectA_Require = selectA_Require;
        this.selectB_Text = selectB_Text;
        this.selectB_JumpID = selectB_JumpID;
        this.selectB_Require = selectB_Require;
        this.selectC_Text = selectC_Text;
        this.selectC_JumpID = selectC_JumpID;
        this.selectC_Require = selectC_Require;
    }

}