using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_AppearMoveDialog : MonoBehaviour
{
    public GameObject MoveDialog;
    [SerializeField] GameManager_MainScene GM;
    [SerializeField] GameManager_MapManage GM_M;
    [SerializeField] GameManager_TimeScale GM_T;
    [SerializeField] GameObject window;

    public void Start()
    {
        GM = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MainScene>();
        GM_M = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MapManage>();
        GM_T = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_TimeScale>();
    }

    public void OnClickStartButton()
    {

        if(GM.Phase == GameManager_MainScene.Phases.IDLE){
            StartCoroutine(callWindow());
        }

    }

    IEnumerator callWindow()
    {
        window = Instantiate(MoveDialog);
        window.transform.SetParent(this.transform.parent.parent.parent.parent);
        while (window != null){
            yield return null;
        }
    }

}
