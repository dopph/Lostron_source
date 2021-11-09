using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackTitle : MonoBehaviour
{
    private GameManager_TimeScale GM_T;

    void Start(){
        GM_T = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_TimeScale>();
    }

    public void BackToTitle(){
        GM_T.unTimeStop(true);
        SceneManager.LoadScene("Title");
    }
}
