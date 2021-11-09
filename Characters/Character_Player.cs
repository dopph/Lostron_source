using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Linq;

public class Character_Player : Character
{
    private GameObject hukidashi;
    [SerializeField] AudioClip voice;
    [SerializeField] GameObject UI_Body;
    public int defaultHP;
    private Animator animator;

    void Start(){
        StatusReset(true);
        isDown = false;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        base.FU();
    }
    
    void Update()
    {
        if(hukidashi != null){
            hukidashi.transform.Find("Canvas/AnimObject/Base").GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y+38);
            hukidashi.transform.Find("Canvas/AnimObject/Arrow").GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y+38);
        }

        if(GM_I.GetItemDataList().Count(item => item.getSortNo() == getNumber()) != 0){
            transform.Find("SpriteSet/Weapon_Rifle").gameObject.GetComponent<SpriteRenderer>().sprite = 
                GM_I.GetItemDataList().First(item => item.getSortNo() == getNumber()).getSprite();
        }
        if(GM_I.GetItemDataList().Count(item => item.getSortNo() == getNumber()) != 0){
            transform.Find("SpriteSet/Weapon_Pistol").gameObject.GetComponent<SpriteRenderer>().sprite = 
                GM_I.GetItemDataList().First(item => item.getSortNo() == getNumber()).getSprite();
        }

        if(GM.Phase == GameManager_MainScene.Phases.IDLE){
            animator.SetBool("Idle", true);
            animator.SetBool("Idle_Rifle", false);
            animator.SetBool("Idle_Pistol", false);
            animator.SetBool("Idle_Shield", false);
        }else if(GM.Phase != GameManager_MainScene.Phases.IDLE){
            if(GM_I.GetItemDataList().Count(item => item.getSortNo() == getNumber()) == 1){

                Item UseItem = GM_I.GetItemDataList().First(item => item.getSortNo() == getNumber());
                if(Shielding.Count == 0){
                    if(UseItem.getAnimName() != ""){
                        animator.SetBool("Idle_Rifle", false);
                        animator.SetBool("Idle_Pistol", false);
                        animator.SetBool("Idle_"+UseItem.getAnimName(), true);
                    }else{
                        animator.SetBool("Idle_Rifle", false);
                        animator.SetBool("Idle_Pistol", false);
                        animator.SetBool("Idle", true);
                    }
                }else{
                    animator.SetBool("Idle_Rifle", false);
                    animator.SetBool("Idle_Pistol", false);
                    animator.SetBool("Idle_Shield", true);
                }
            }
        }
    }

    public override int getATK(){
        if(GM_I.GetItemDataList().Count(item => item.getSortNo() == getNumber()) == 1){
            Item UseItem = GM_I.GetItemDataList().First(item => item.getSortNo() == getNumber());
            return UseItem.getDamage();
        }
        return 1;
    }

    public override int getHit(){
        int Value = 0;

        if(GM_I.GetItemDataList().Count(item => item.getSortNo() == getNumber()) == 1){
            Item UseItem = GM_I.GetItemDataList().First(item => item.getSortNo() == getNumber());
            Value += UseItem.getHit();
        }
        Value += HitReget_;
        return Value;
    }

    public void StopShotAnim(){
        animator.SetBool("Fire", false);
    }

    public override void TakeDamage(int Damage, bool motion = true){
        base.TakeDamage(Damage, motion);
        if(Damage >= 0 && motion == true)animator.SetBool("Damage", true);
    }

    public override void Down(){
        base.Down();
        animator.SetBool("Die", true);
    }

    public GameObject getUIBody(){
        return UI_Body;
    }

    public override GameManager_MainScene.Passives getPassive(){

        int ID;
        if(GM_I.GetItemDataList().Count(item => item.getSortNo() == getNumber()+3) == 0){
            return GameManager_MainScene.Passives.None;
        }else{
            ID = GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == getNumber()+3).getID();
        }

        if(ID == 106001)return GameManager_MainScene.Passives.Scope;
        if(ID == 106002)return GameManager_MainScene.Passives.Armor;
        if(ID == 106003)return GameManager_MainScene.Passives.AIvisor;
        if(ID == 106004)return GameManager_MainScene.Passives.PierceAmmo;
        if(ID == 106005)return GameManager_MainScene.Passives.PierceAmmoPlus;
        if(ID == 106007)return GameManager_MainScene.Passives.NanoBot;

        return GameManager_MainScene.Passives.None;
    }

}
