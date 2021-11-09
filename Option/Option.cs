using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{

    private Slider Slider;
    [SerializeField] string Name;
    private int Volume;
    [SerializeField] Text ValueText;

    void Start(){

        Slider = GetComponent<Slider>();
        Volume = ES3.Load<int>(Name+"_Volume", defaultValue:50);
        Debug.Log(Volume);
        Slider.value = Volume;
        ValueText.text = Slider.value.ToString();
        
    }

    void Update(){
    }

    public void ChangeBGM_Volume(){
        StartItem.BGM_Volume = (int)Slider.value;
        ES3.Save<int>("BGM_Volume", (int)Slider.value);
        ValueText.text = Slider.value.ToString();
    }
    
    public void ChangeSE_Volume(){
        StartItem.SE_Volume = (int)Slider.value;
        ES3.Save<int>("SE_Volume", (int)Slider.value);
        ValueText.text = Slider.value.ToString();
    }

    public void SaveOption(){

    }
}
