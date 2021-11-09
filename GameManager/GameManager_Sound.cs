using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Sound : MonoBehaviour
{

    public GameManager_MainScene GM;
    public GameManager_MapManage GM_M;
    public AudioSource Idle_Ruin;
    public AudioSource Battle_Ruin;
    public AudioSource Idle_Base;
    public AudioSource Battle_Base;
    public AudioSource Battle_Boss;

    public AudioSource GameOver;

    private int MaxVolume;
    public int maxVolume{
        get {return MaxVolume;}
        set {MaxVolume = value;}
    }

    private int onPlay = 0;
    private AudioSource PlayingBGM;

    void Start(){
        PlayingBGM = Idle_Ruin;
    }

    private void AllBGMReset(){
        Idle_Ruin.time = 0f;
        Battle_Ruin.time = 0f;
        Idle_Base.time = 0f;
        Battle_Base.time = 0f;
        Battle_Boss.time = 0f;
    }

    private IEnumerator BGM(AudioSource BGM, bool PauseType){
        PlayingBGM = BGM;
        if(onPlay == 0 || (onPlay == 1 && PauseType == false)){

            onPlay = 2;

            while(PlayingBGM == BGM){
                if(PauseType == true){
                    BGM.UnPause();
                }else{
                    BGM.enabled = true;
                }
                BGM.volume += 0.65f * MaxVolume/100f * Time.unscaledDeltaTime;
                if(BGM.volume > MaxVolume/100f)BGM.volume = MaxVolume/100f;
                yield return null;
            }

            onPlay = 1;
            if(PauseType == false)onPlay = 0;

            while(true){
                BGM.volume -= 0.65f * MaxVolume/100f * Time.unscaledDeltaTime;
                if(BGM.volume <= 0){
                    BGM.volume = 0;
                    if(PauseType == true){
                        BGM.Pause();
                        if(onPlay == 1)AllBGMReset();
                    }else{
                        BGM.enabled = false;
                    }
                    onPlay = 0;
                    yield break;
                }
                yield return null;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(GM.Phase == GameManager_MainScene.Phases.IDLE && GM_M.currentHierarchy < 10){
            StartCoroutine(BGM(Idle_Ruin, true));
        }else if(GM.Phase == GameManager_MainScene.Phases.IN_BATTLE && GM_M.currentHierarchy < 10){
            StartCoroutine(BGM(Battle_Ruin, false));
        }else if(GM_M.currentHierarchy == 10 || GM_M.currentHierarchy == 20){
            StartCoroutine(BGM(Battle_Boss, true));
        }else if(GM.Phase == GameManager_MainScene.Phases.IDLE && GM_M.currentHierarchy > 10){
            StartCoroutine(BGM(Idle_Base, true));
        }else if(GM.Phase == GameManager_MainScene.Phases.IN_BATTLE && GM_M.currentHierarchy > 10){
            StartCoroutine(BGM(Battle_Base, false));
        }
    }
}
