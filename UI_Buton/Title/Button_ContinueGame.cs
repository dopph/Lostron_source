using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button_ContinueGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(ES3.KeyExists("rooms") == false){
            this.gameObject.GetComponent<Button>().enabled = false;
            this.gameObject.GetComponent<Animator>().SetBool("Disabled", true);
        }

    }

    public void OnClickStartButton()
    {
        StartItem.isContinue = true;
        SceneManager.LoadScene("MainScene");
    }
}
