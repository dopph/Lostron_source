using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOption : MonoBehaviour
{

    public bool isBGM;

    void Start(){
        StartItem.BGM_Volume = ES3.Load<int>("BGM_Volume", defaultValue:50);
        StartItem.SE_Volume = ES3.Load<int>("SE_Volume", defaultValue:50);
    }
    
    void Update()
    {

        if(isBGM){
            if(GetComponent<GameManager_Sound>() == null)GetComponent<AudioSource>().volume = (float)StartItem.BGM_Volume/100;
                else GetComponent<GameManager_Sound>().maxVolume = (int)StartItem.BGM_Volume;
        }else{
            GetComponent<AudioSource>().volume = (float)StartItem.SE_Volume/100;
        }
    }

}
