using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Button_OpenMenu : MonoBehaviour
{
    private GameObject window;
    [SerializeField] GameObject Menu;

    public void OnClickStartButton(){
        window = Instantiate(Menu);
        window.transform.SetParent(this.transform.parent.parent.parent.parent);
    }
 
    void Update(){
        if(window != null){
            //Time.timeScale = 0f;
        }else{
            //Time.timeScale = 1f;
        }

    }
}
