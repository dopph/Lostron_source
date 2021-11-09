using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Button_OpenOption : MonoBehaviour
{
    public GameObject window;

    public void OnClickStartButton(){
        GameObject i;
        i = Instantiate(window);
        i.transform.SetParent(this.transform.parent.parent.parent);
    }
 
}
