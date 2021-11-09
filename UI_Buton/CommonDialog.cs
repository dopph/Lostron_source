using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CommonDialog : MonoBehaviour
{

    public bool OnlyOKButton;
    public bool isChoiceDialog;
    private int Value;
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;

    public GameObject TitleText;
    public GameObject MainText;
    public GameObject Btn1Text;
    public GameObject Btn2Text;
    public GameObject Btn3Text;

    void Awake(){
        SetValue(-1);
    }

    void Start(){
        if(isChoiceDialog == false){
            if(OnlyOKButton == true){
                Button1.SetActive(false);
                Button2.SetActive(false);
            }else{
                Button3.SetActive(false);
            }
        }
    }
    
    public void SetValue(int Value){
        this.Value = Value;
    }
    public int GetValue(){
        return this.Value;
    }
    public void SetTitleText(string txt){
        this.TitleText.GetComponent<Text>().text = txt;
    }
    public void SetMainText(string txt){
        this.MainText.GetComponent<Text>().text = txt;
    }

    public void SetBtn1Text(string txt){
        this.Btn1Text.GetComponent<Text>().text = txt;
    }
    public void SetBtn2Text(string txt){
        this.Btn2Text.GetComponent<Text>().text = txt;
    }
    public void SetBtn3Text(string txt){
        this.Btn3Text.GetComponent<Text>().text = txt;
    }
}
