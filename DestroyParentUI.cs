using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParentUI : MonoBehaviour
{
    public GameObject window;
    private GameManager_TimeScale GM_T;
    
    public void DestroyParent(){
        Destroy(this.window);
    }

    public void DisableParent(){
        this.window.SetActive(false);
    }

    public void stopTime(){
        GM_T = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_TimeScale>();
        GM_T.setTimeStop();
    }

    public void unStopTime(){
        GM_T = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_TimeScale>();
        GM_T.unTimeStop();
    }

}
