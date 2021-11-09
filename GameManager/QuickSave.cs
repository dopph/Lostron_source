using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class QuickSave : MonoBehaviour
{
    
    private GameManager_MainScene GM;
    private GameManager_ItemManage GM_I;
    private GameManager_MapManage GM_M;
    private GameManager_TimeScale GM_T;

    // Start is called before the first frame update
    void Start()
    {
        GM = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MainScene>();
        GM_I = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_ItemManage>();
        GM_M = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MapManage>();
        GM_T = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_TimeScale>();
    }

    void Update(){
        if(GM.Phase == GameManager_MainScene.Phases.IDLE){
            this.gameObject.GetComponent<Button>().enabled = true;
            this.gameObject.GetComponent<Animator>().SetBool("Disabled", false);
        }else{
            this.gameObject.GetComponent<Button>().enabled = false;
            this.gameObject.GetComponent<Animator>().SetBool("Disabled", true);
        }
    }

    // Update is called once per frame
    public void Save()
    {
        ES3.Save<int>("chara1ID", GM.Players[0].GetComponent<Character>().myPrefabID);
        ES3.Save<int>("chara1HP", GM.Players[0].GetComponent<Character>().getHP());
        ES3.Save<int>("chara2ID", GM.Players[1].GetComponent<Character>().myPrefabID);
        ES3.Save<int>("chara2HP", GM.Players[1].GetComponent<Character>().getHP());
        ES3.Save<int>("chara3ID", GM.Players[2].GetComponent<Character>().myPrefabID);
        ES3.Save<int>("chara3HP", GM.Players[2].GetComponent<Character>().getHP());
        ES3.Save<List<Item>>("item", GM_I.GetItemDataList());
        ES3.Save<int>("currentLocale", GM_M.currentLocale);
        ES3.Save<int>("currentHierarchy", GM_M.currentHierarchy);
        ES3.Save<List<Room>>("rooms", GM_M.Rooms);

        GM_T.unTimeStop(true);
        SceneManager.LoadScene("Title");
    }
}
