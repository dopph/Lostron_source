using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public partial class GameManager_MainScene
{

    public enum Mods
    {
        HitRateHarf,
        DodgeRateZero,
        DodgeRateDouble,
        MissRateDouble,
        MissRateZero
    }

    public enum Passives
    {
        None,
        Scope,
        AIvisor,
        Armor,
        PierceAmmo,
        PierceAmmoPlus,
        NanoBot
    }

    public void AllStatusReset(){
        Players[0].GetComponent<Character>().StatusReset();
        Players[1].GetComponent<Character>().StatusReset();
        Players[2].GetComponent<Character>().StatusReset();
        Enemies[0].GetComponent<Character>().StatusReset();
        Enemies[1].GetComponent<Character>().StatusReset();
        Enemies[2].GetComponent<Character>().StatusReset();
    }

    public GameObject AutoSelectTarget(GameObject Actor, int tgt = -1, bool Team = false){
        if(tgt == -1){
            if(Actor.GetComponent<Character>().getTeam() == Team){
                if(Enemies[0].GetComponent<Character>().isDown == false){
                    return Enemies[0];
                }else if(Enemies[1].GetComponent<Character>().isDown == false){
                    return Enemies[1];
                }else{
                    return Enemies[2];
                }
            }else{
                if(Players[0].GetComponent<Character>().isDown == false){
                    return Players[0];
                }else if(Players[1].GetComponent<Character>().isDown == false){
                    return Players[1];
                }else{
                    return Players[2];
                }
            }
        }else{
            if(Actor.GetComponent<Character>().getTeam() == Team){
                if(Enemies[tgt].GetComponent<Character>().isDown == false){
                    return Enemies[tgt];
                }else{
                    return null;
                }
            }else{
                if(Players[tgt].GetComponent<Character>().isDown == false){
                    return Players[tgt];
                }else{
                    return null;
                }
            }
        }
    } 

    public GameObject AISelectTarget(GameObject Actor, bool Team = false){
        int Rand = Random.Range(0, 10)+1;
        if(Actor.GetComponent<Character>().getTeam() == Team){
            if(Rand <= 6)return Enemies[0];
            if(Rand <= 9)return Enemies[1];
            return Enemies[2];
        }else{
            if(Rand <= 6)return Players[0];
            if(Rand <= 9)return Players[1];
            return Players[2];
        }
    } 

    public int Attack(Character Actor, Character Target, int DamageF, float DamageMulti, int HitRate, List<Mods> Mod){

        ActionData actionData = new ActionData(Actor, Target, DamageF, DamageMulti, HitRate, Mod);
        int Damage = Random.Range(actionData.MinDamage, actionData.MaxDamage);
        Actor.attackCount += 1;

        if(Random.Range(0,100) < actionData.FinalHitRate){
            Target.TakeDamage(Damage);
            if(Target.getPassive() == GameManager_MainScene.Passives.Armor && Random.Range(0,100) < 18){
                GM_I.DeleteItem(Target.getNumber()+3);
                GameObject Pop = Instantiate(Target.DamagePopup);
                Pop.transform.SetParent(Target.DamagePopupParent.transform);
                Pop.transform.localScale = new Vector3(1f, 1f, 1f);
                Pop.transform.Find("Text_Num_01").GetComponent<Text>().text = "アーマーが壊れた";
            }
            return Damage;
        }else{
            Target.TakeDamage(-1);
            return 0;
        }
    }

    public ActionData getActionData(int Command, GameObject ActorObj, bool isBurst, int getDataOnly = -1){
        var coroutine = Action(Command, ActorObj, Enemies[getDataOnly], isBurst, getDataOnly);
        StartCoroutine( coroutine );
        return (ActionData)coroutine.Current;
    }
    
}

public class ActionData{

    public int MinDamage;
    public int MaxDamage;
    public int FinalHitRate;

    public ActionData(Character Actor, Character Target, int DamageF, float DamageMulti, int HitRate, List<GameManager_MainScene.Mods> Mod){

        int DodgeRate = 0;
        int Shield = 0;
        float Graze = Target.DodgeReget_;
        int Damage = DamageF;

        foreach(int value in Target.Graze){
            Graze += value;
        }
        foreach(int value in Actor.HitRate){
            HitRate += value;
        }
        foreach(int value in Target.DodgeRate){
            DodgeRate += value;
        }
        foreach(int value in Target.Shielding){
            Shield += value;
        }

        if(Target.getPassive() == GameManager_MainScene.Passives.AIvisor){
            DodgeRate += 10;
        }
        if(Actor.getPassive() == GameManager_MainScene.Passives.Scope){
            HitRate += 10;
            Graze = 0;
        }
        if(Actor.getPassive() == GameManager_MainScene.Passives.PierceAmmo){
            Damage += 2;
        }
        if(Actor.getPassive() == GameManager_MainScene.Passives.PierceAmmoPlus){
            Damage += 4;
            Shield = 0;
        }
        if(Target.getPassive() == GameManager_MainScene.Passives.Armor){
            Shield += 1;
        }

        MaxDamage = (int)Math.Round(Damage*DamageMulti, MidpointRounding.AwayFromZero);
        MaxDamage -= Shield;

        if(MaxDamage < 1)MaxDamage = 1;

        if(Graze > 1)Graze = 1;
        MinDamage = (int)Math.Round(MaxDamage*(1.0f-Graze), MidpointRounding.AwayFromZero);

        if(Mod.IndexOf(GameManager_MainScene.Mods.MissRateDouble) != -1)HitRate = 100 - ((100-HitRate)*2);
        if(Mod.IndexOf(GameManager_MainScene.Mods.MissRateZero) != -1)HitRate = 100;
        if(Mod.IndexOf(GameManager_MainScene.Mods.DodgeRateZero) != -1){
            MinDamage = MaxDamage;
            if(DodgeRate > 0)DodgeRate = 0;
        }

        if(Mod.IndexOf(GameManager_MainScene.Mods.DodgeRateDouble) != -1)DodgeRate *= 2;
        if(MinDamage > MaxDamage)MinDamage = MaxDamage;
        if(MinDamage < 1) MinDamage = 1;

        FinalHitRate = HitRate - DodgeRate;
        if(FinalHitRate < 0)FinalHitRate = 0;
        if(FinalHitRate > 100)FinalHitRate = 100;
    }

}