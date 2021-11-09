using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ActionCard
{
    public GameManager_MainScene GameManager;
    private int ID;
    private string Name;
    private string Explain;
    private string CritExplain;
    private string Address; 
    private int TargetType;
    private Sprite CardSprite;
    public Sprite cardSprite{get{return CardSprite;}}

    public ActionCard(int ID, string Name, string Explain, string CritExplain, string Address, int TargetType){
        this.ID = ID;
        this.Name = Name;
        this.Explain = Explain;
        this.CritExplain = CritExplain;
        this.Address = Address;
        this.TargetType = TargetType;

        if(Address != "")
        Addressables.LoadAssetAsync<Sprite>(Address).Completed += handle =>{
            CardSprite = handle.Result;
        };

    }

    public int getID(){
        return ID;
    }
    public string getName(){
        return Name;
    }
    public string getExplain(){
        return Explain;
    }
    public string getCritExplain(){
        return CritExplain;
    }
    public string getAddress(){
        return Address;
    }
    public int getTargetType(){
        return TargetType;
    }
}
