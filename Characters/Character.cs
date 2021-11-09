using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class Character : MonoBehaviour , ICharacter
{
    
    public GameManager_MainScene GM;
    public GameManager_ItemManage GM_I;
    public int myPrefabID;
    
    private string Name; 
    private int MaxHP; 
    private int HP; 
    private List<int> Ideas; 
    private int Command;
    private bool isCrit;
    private GameObject Target;
    private int Number;
    private bool Team;
    public bool isDown;
    public bool isTargeted;


    public int waitTime = 0; 
    public GameObject DamagePopupParent;
    public GameObject DamagePopup;

    public List<int> DodgeRate;
    public List<int> Graze;
    public List<int> Shielding;
    public List<int> HitRate;

    public List<int> DodgeRate_Turn;
    public List<int> Graze_Turn;
    public List<int> HitRate_Turn;
    public List<int> Shielding_Turn;

    public int CantDodge;
    public int DodgeRate_Past;
    public int Graze_Past;
    public int Stun;

    [SerializeField] int IdeaPlus;
    public int IdeaPlus_ {
        get {return IdeaPlus;}
        set {IdeaPlus = value;}
    }

    [SerializeField] int DodgeReget;
    public int DodgeReget_{
        get{return DodgeReget;}
        set{DodgeReget = value;}
    }

    [SerializeField] int HitReget;
    public int HitReget_{
        get{return HitReget;}
        set{HitReget = value;}
    }

    private int AttackCount;
    public int attackCount {
        get {return AttackCount;}
        set {AttackCount = value;}
    }

    [SerializeField] GameObject HealEffect;

    public void DodgeRateBuff(int Value, int Turn){
        DodgeRate.Add(Value);
        DodgeRate_Turn.Add(Turn);

        GameObject Pop = Instantiate(DamagePopup);
        Pop.transform.SetParent(DamagePopupParent.transform);
        Pop.transform.localScale = new Vector3(1f, 1f, 1f);
        if(Value > 0){
            Pop.transform.Find("Text_Num_01").GetComponent<Text>().text = "回避+"+(Value.ToString());
        }else{
            Pop.transform.Find("Text_Num_01").GetComponent<Text>().text = "回避"+(Value.ToString());
        }
    }
    public void GrazeBuff(int Value, int Turn){
        Graze.Add(Value);
        Graze_Turn.Add(Turn);
    }
    public void HitRateBuff(int Value, int Turn){
        HitRate.Add(Value);
        HitRate_Turn.Add(Turn);
        
        GameObject Pop = Instantiate(DamagePopup);
        Pop.transform.SetParent(DamagePopupParent.transform);
        Pop.transform.localScale = new Vector3(1f, 1f, 1f);
        if(Value > 0){
            Pop.transform.Find("Text_Num_01").GetComponent<Text>().text = "命中率+"+(Value.ToString());
        }else{
            Pop.transform.Find("Text_Num_01").GetComponent<Text>().text = "命中率"+(Value.ToString());
        }
    }
    public void ShieldingBuff(int Value, int Turn){
        Shielding.Add(Value);
        Shielding_Turn.Add(Turn);
        GameObject Pop = Instantiate(DamagePopup);
        Pop.transform.SetParent(DamagePopupParent.transform);
        Pop.transform.localScale = new Vector3(1f, 1f, 1f);
        if(Value > 0){
            Pop.transform.Find("Text_Num_01").GetComponent<Text>().text = "盾+"+(Value.ToString());
        }else{
            Pop.transform.Find("Text_Num_01").GetComponent<Text>().text = "盾"+(Value.ToString());
        }
    }

    public void StatusReset(bool isStart = false){

        if((getPassive() == GameManager_MainScene.Passives.PierceAmmo || getPassive() == GameManager_MainScene.Passives.PierceAmmoPlus) && attackCount > 0 && isStart == false){
            GM_I.DeleteItem(getNumber()+3);
        }

        DodgeRate = new List<int>();
        Graze = new List<int>();
        Shielding = new List<int>();
        HitRate = new List<int>();
        DodgeRate_Turn = new List<int>();
        Graze_Turn = new List<int>();
        HitRate_Turn = new List<int>();
        Shielding_Turn = new List<int>();
        DodgeRate_Past = 0;
        Graze_Past = 0;
        CantDodge = 0;
        Stun = 0;
        isTargeted = false;
        isCrit = false;
        attackCount = 0;
    }

    public void ConsumeTurnEffect(bool hit = true, bool dod = true, bool def = true, bool tgt = false){

        if(hit == true){
            for(int i = 0; i < HitRate.Count; i++){
                if(HitRate_Turn[i] > 0)HitRate_Turn[i]--;
                if(HitRate_Turn[i] == 0){
                    HitRate.RemoveAt(i);
                    HitRate_Turn.RemoveAt(i);
                }
            }
        }
        if(dod == true){
            DodgeRate_Past = 0;
            foreach(int value in DodgeRate){
                DodgeRate_Past += value;
            }
            for(int i = 0; i < DodgeRate.Count; i++){
                if(DodgeRate_Turn[i] > 0)DodgeRate_Turn[i]--;
                if(DodgeRate_Turn[i] == 0){
                    DodgeRate.RemoveAt(i);
                    DodgeRate_Turn.RemoveAt(i);
                }
            }

            Graze_Past = 0;
            foreach(int value in Graze){
                Graze_Past += value;
                if(Graze_Past > 1)Graze_Past = 1;
            }
            for(int i = 0; i < Graze.Count; i++){
                if(Graze_Turn[i] > 0)Graze_Turn[i]--;
                if(Graze_Turn[i] == 0){
                    Graze.RemoveAt(i);
                    Graze_Turn.RemoveAt(i);
                }
            }

        }
        if(def == true){
            for(int i = 0; i < Shielding.Count; i++){
                if(Shielding_Turn[i] > 0)Shielding_Turn[i]--;
                if(Shielding_Turn[i] == 0){
                    Shielding.RemoveAt(i);
                    Shielding_Turn.RemoveAt(i);
                }
            }
        }
        if(tgt == true){
            isTargeted = false;
        }else{
            if(CantDodge > 0)CantDodge--;
            if(Stun > 0)Stun--;
        }
    }

    void Start(){
        //StatusReset(true);
        //isDown = false;
    }

    void FixedUpdate()
    {
        FU();
    }

    public void FU(){
        if(!isDown){
            if(isTargeted){
                transform.Find("CharaData/Canvas/Target_Object").GetComponent<Animator>().SetBool("Targeted", true);
            }else{
                transform.Find("CharaData/Canvas/Target_Object").GetComponent<Animator>().SetBool("Targeted", false);
            }
        }
    }

    public void setNumber(int n){
        Number = n;
    }
    public int getNumber(){
        return Number;
    }

    public int getHP(){
        return HP;
    } 
    public int getMaxHP(){
        return MaxHP;
    } 
    
    public void setHP(int HP, int MaxHP){
        this.HP = HP;
        this.MaxHP = MaxHP;
    } 

    public bool getTeam(){
        return Team;
    } 
    public void setTeam(bool Value){
        Team = Value;
    } 

    public int getCommand(){
        return Command;
    } 
    public void setCommand(int Value){
        Command = Value;
    } 

    public bool getisCrit(){
        return isCrit;
    } 
    public void setIsCrit(bool Value){
        isCrit = Value;
    } 

    public virtual int getATK(){
        return 3;
    }
    public virtual int getHit(){
        return 90;
    }

    public bool getIsTargeted(){
        return isTargeted;
    }
    public void setIsTargeted(bool Value){
        isTargeted = Value;
    }

    public void setTarget(GameObject Target){
        this.Target = Target;
    }
    public GameObject getTarget(){
        return Target;
    }
    
    public virtual void TakeDamage(int Damage, bool motion = true){

        GameObject Pop = Instantiate(DamagePopup);
        Pop.transform.SetParent(DamagePopupParent.transform);
        Pop.transform.localScale = new Vector3(1f, 1f, 1f);

        if(Damage >= 0){
            Pop.transform.Find("Text_Num_01").GetComponent<Text>().text = Damage.ToString();
            HP -= Damage;

            if(HP <= 0){
                if(motion == false){
                    HP = 1;
                }else{
                    Down();
                    GM.OpenCharaSlot();
                }
            }
        }else{
            Pop.transform.Find("Text_Num_01").GetComponent<Text>().text = "Miss!";
        }

    }

    public virtual void Heal(int Value){
        HP += Value;
        if(HP > MaxHP){
            HP = MaxHP;
        }
    }

    public void TakeHeal(int Value){
        HealEffect.SetActive(true);
        Heal(Value);
    }

    public virtual void Down(){
        isDown = true;
    }

    public virtual GameManager_MainScene.Passives getPassive(){
        return GameManager_MainScene.Passives.None;
    }
}

public class Idea{

    private GameManager_MainScene GameManager;
    private int ID;

}