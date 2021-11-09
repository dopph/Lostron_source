using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_TimeScale : MonoBehaviour
{
    private int TimeStopper = 0;

    void Update(){
        if(TimeStopper == 0){
            Time.timeScale = 1f;
        }else{
            Time.timeScale = 0f;
        }
    }

    public void setTimeStop(){
        TimeStopper++;
    }
    public void unTimeStop(bool absolute = false){
        TimeStopper--;
        if(absolute == true){
            TimeStopper = 0;
            Time.timeScale = 1f;
        }
    }
    public int getTimeScale(){
        return TimeStopper;
    }
}
