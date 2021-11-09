using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class GameManager_MainScene
{

    private IEnumerator Action(int Command, GameObject ActorObj, GameObject TargetObj, bool isBurst, int getDataOnly = -1){
        
        Character Actor = ActorObj.GetComponent<Character>();
        Character Target = TargetObj.GetComponent<Character>();

        if(Target.isDown == true){
            TargetObj = AutoSelectTarget(ActorObj);
            Target = TargetObj.GetComponent<Character>();
        }
        int AttackDamage = Actor.getATK();
        int HitRate = Actor.getHit();
        int Result;
        bool isCrit = Actor.getisCrit();

        if(Command == 0){ //通常攻撃
            if(isBurst){
                yield return null;
            }else{
                yield return null;
            }

        }else if(Command == 1){ //シールド
            if(getDataOnly != -1){yield return null; yield break;}
            if(isBurst){
                Debug.Log("盾を構えた！");
                Debug.Log("シールド2獲得。");
                Actor.ShieldingBuff(2, 1);
                yield return new WaitForSeconds(2f);
            }else{
                yield return null;
            }

        }else if(Command == 2){ //さがる
        if(getDataOnly != -1){yield return null; yield break;}
            if(isBurst){
                if(isCrit)Actor.DodgeRateBuff(40, 1);
                    else Actor.DodgeRateBuff(30, 1);
                Actor.GrazeBuff(1, 1);
                yield return new WaitForSeconds(2f);
            }else{
                yield return null;
            }

        }else if(Command == 3){ //武装解除
            if(getDataOnly != -1){
                if(isCrit)yield return new ActionData(Actor, Target, AttackDamage, 0.5f, HitRate, new List<Mods>());
                    else yield return new ActionData(Actor, Target, AttackDamage, 0.5f, HitRate, new List<Mods>(){ Mods.MissRateDouble });
                yield break;
            }
            if(isBurst){
                Debug.Log("相手の武装に攻撃！");
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                if(isCrit)Result = Attack(Actor, Target, AttackDamage, 0.5f, HitRate, new List<Mods>(){});
                    else Result = Attack(Actor, Target, AttackDamage, 0.5f, HitRate, new List<Mods>(){ Mods.MissRateDouble });
                if(Result > 0){
                    Debug.Log("相手は命中率30ダウン。");
                    Target.HitRateBuff(-30, 1);
                }
                yield return new WaitForSeconds(1.5f);
            }else{
                yield return null;
            }

        }else if(Command == 4){ //腰だめ
            if(getDataOnly != -1){
                yield return new ActionData(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>());
                yield break;
            }
            if(isBurst){
                if(isCrit)Actor.DodgeRateBuff(30, 1);
                    else Actor.DodgeRateBuff(15, 1);
                Actor.GrazeBuff(30, 1);
                yield return new WaitForSeconds(2f);
            }else{
                Debug.Log("腰だめうち！");
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>());
                yield return new WaitForSeconds(1.5f);
            }
        }else if(Command == 5){ //クイックドロー
            if(getDataOnly != -1){
                if(isCrit)yield return new ActionData(Actor, Target, AttackDamage, 1.2f, HitRate, new List<Mods>());
                    else yield return new ActionData(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>());
                yield break;
            }
            if(isBurst){
                Debug.Log("クイックドロー！");
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                if(isCrit)Attack(Actor, Target, AttackDamage, 1.2f, HitRate, new List<Mods>());
                    else Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>());
                yield return new WaitForSeconds(1f);
            }else{
                yield return null;
            }

        }else if(Command == 6){ //グレネード
            if(getDataOnly != -1){
                int dmg;
                if(isCrit)dmg = 10; else dmg = 5;
                TargetObj = AutoSelectTarget(ActorObj, 0);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    if(getDataOnly == 0){yield return new ActionData(Actor, Target, 5+dmg, 1f, 100, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateZero }); yield break;}
                    dmg = 0;
                }
                TargetObj = AutoSelectTarget(ActorObj, 1);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    if(getDataOnly == 1){yield return new ActionData(Actor, Target, 5+dmg, 1f, 100, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateZero }); yield break;}
                    dmg = 0;
                }
                TargetObj = AutoSelectTarget(ActorObj, 2);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    if(getDataOnly == 2){yield return new ActionData(Actor, Target, 5+dmg, 1f, 100, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateZero }); yield break;}
                    dmg = 0;
                }
                yield break;
            }

            if(isBurst){
                yield return null;
            }else{
                Debug.Log("グレネードをなげた！");
                yield return new WaitForSeconds(0.1f);
                ActorObj.GetComponent<Animator>().SetBool("Fire_Grenade", true);
                yield return new WaitForSeconds(1.15f);

                int dmg;
                if(isCrit)dmg = 10; else dmg = 5;

                TargetObj = AutoSelectTarget(ActorObj, 0);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Attack(Actor, Target, 5+dmg, 1f, 100, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateZero });
                    dmg = 0;
                }

                yield return new WaitForSeconds(0.1f);
                TargetObj = AutoSelectTarget(ActorObj, 1);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Attack(Actor, Target, 5+dmg, 1f, 100, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateZero });
                    dmg = 0;
                }

                yield return new WaitForSeconds(0.1f);
                TargetObj = AutoSelectTarget(ActorObj, 2);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Attack(Actor, Target, 5+dmg, 1f, 100, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateZero });
                    dmg = 0;
                }

                GM_I.DeleteItem(Actor.getNumber()+3);

                yield return new WaitForSeconds(1f);
            }

        }else if(Command == 7){ //フラッシュバン
            if(getDataOnly != -1){yield return null; yield break;}
            if(isBurst){
                yield return null;
            }else{
                Debug.Log("フラッシュバン！");
                yield return new WaitForSeconds(0.1f);
                ActorObj.GetComponent<Animator>().SetBool("Fire_Grenade", true);
                yield return new WaitForSeconds(1.15f);
                int dmg = 10;

                TargetObj = AutoSelectTarget(ActorObj, 0);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Attack(Actor, Target, 5+dmg, 1f, 100, new List<Mods>(){ Mods.DodgeRateDouble });
                    dmg = 0;
                }

                yield return new WaitForSeconds(0.1f);
                TargetObj = AutoSelectTarget(ActorObj, 1);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Attack(Actor, Target, 5+dmg, 1f, 100, new List<Mods>(){ Mods.DodgeRateDouble });
                    dmg = 0;
                }

                yield return new WaitForSeconds(0.1f);
                TargetObj = AutoSelectTarget(ActorObj, 2);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Attack(Actor, Target, 5+dmg, 1f, 100, new List<Mods>(){ Mods.DodgeRateDouble });
                    dmg = 0;
                }

                GM_I.DeleteItem(Actor.getNumber()+3);

                yield return new WaitForSeconds(1f);
            }

        }else if(Command == 8){ //ヘッドショット
            if(getDataOnly != -1){
                if(isCrit)yield return new ActionData(Actor, Target, AttackDamage, 2f, HitRate, new List<Mods>());
                    else yield return new ActionData(Actor, Target, AttackDamage, 1.5f, HitRate, new List<Mods>());
                yield break;
            }
            if(isBurst){
                yield return null;
            }else{
                Debug.Log("ヘッドショット！");
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                if(isCrit)Attack(Actor, Target, AttackDamage, 2f, HitRate, new List<Mods>());
                    else Attack(Actor, Target, AttackDamage, 1.5f, HitRate, new List<Mods>());
                yield return new WaitForSeconds(1f);
            }

        }else if(Command == 9){ //掃射
            if(getDataOnly != -1){
                if(isCrit)yield return new ActionData(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateDouble });
                    else yield return new ActionData(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateDouble });
                yield break;
            }
            if(isBurst){
                yield return null;
            }else{
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                TargetObj = AutoSelectTarget(ActorObj, 0);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    if(isCrit)Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateDouble });
                        else Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateDouble });
                }

                yield return new WaitForSeconds(0.1f);
                TargetObj = AutoSelectTarget(ActorObj, 1);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    if(isCrit)Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateDouble });
                        else Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateDouble });
                }

                yield return new WaitForSeconds(0.1f);
                TargetObj = AutoSelectTarget(ActorObj, 2);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    if(isCrit)Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateDouble });
                        else Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateDouble });
                }
                yield return new WaitForSeconds(1f);
            }

        }else if(Command == 10){ //盾撃ち
            if(getDataOnly != -1){
                if(isCrit)yield return new ActionData(Actor, Target, AttackDamage, 0.8f, HitRate, new List<Mods>());
                    else yield return new ActionData(Actor, Target, AttackDamage, 0.5f, HitRate, new List<Mods>(){ Mods.MissRateDouble });
                yield break;
            }
            if(isBurst){
                Debug.Log("盾を構えた！");
                Debug.Log("シールド1獲得。");
                Actor.ShieldingBuff(2, 1);
                yield return new WaitForSeconds(2f);
            }else{
                Debug.Log("盾撃ち！");
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                if(isCrit)Attack(Actor, Target, AttackDamage, 0.8f, HitRate, new List<Mods>());
                    else Attack(Actor, Target, AttackDamage, 0.5f, HitRate, new List<Mods>(){ Mods.MissRateDouble });
                yield return new WaitForSeconds(1.5f);
            }

        }else if(Command == 11){ //精密射撃
            if(getDataOnly != -1){
                if(isCrit)yield return new ActionData(Actor, Target, AttackDamage, 1.2f, HitRate, new List<Mods>(){ Mods.DodgeRateZero, Mods.MissRateZero });
                    else yield return new ActionData(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateZero, Mods.MissRateZero });
                yield break;
            }
            if(isBurst){
                yield return null;
            }else{
                Debug.Log("精密射撃！");
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                if(isCrit)Attack(Actor, Target, AttackDamage, 1.2f, HitRate, new List<Mods>(){ Mods.DodgeRateZero, Mods.MissRateZero });
                    else Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateZero, Mods.MissRateZero });
                yield return new WaitForSeconds(1.5f);
            }

        }else if(Command == 12){ //足狙い
            if(getDataOnly != -1){
                if(isCrit)yield return new ActionData(Actor, Target, AttackDamage, 0.8f, HitRate, new List<Mods>(){ Mods.DodgeRateZero });
                    else yield return new ActionData(Actor, Target, AttackDamage, 0.5f, HitRate, new List<Mods>(){ Mods.DodgeRateZero });
                yield break;
            }
            if(isBurst){
                Debug.Log("相手の足に攻撃！");
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                if(isCrit)Result = Attack(Actor, Target, AttackDamage, 0.8f, HitRate, new List<Mods>(){ Mods.DodgeRateZero });
                    else Result = Attack(Actor, Target, AttackDamage, 0.5f, HitRate, new List<Mods>(){ Mods.DodgeRateZero });
                if(Result > 0){
                    Debug.Log("相手は回避率40ダウン。");
                    Target.DodgeRateBuff(-40, 2);
                }
                yield return new WaitForSeconds(1.5f);
            }else{
                yield return null;
            }

        }else if(Command == 13){ //制圧射撃
            if(getDataOnly != -1){
                yield return new ActionData(Actor, Target, 0, 1f, 100, new List<Mods>(){ Mods.DodgeRateZero, Mods.MissRateZero });
                yield break;
            }
            if(isBurst){
                yield return null;
            }else{
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                TargetObj = AutoSelectTarget(ActorObj, 0);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                }

                TargetObj = AutoSelectTarget(ActorObj, 1);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                }

                TargetObj = AutoSelectTarget(ActorObj, 2);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                }
                yield return new WaitForSeconds(1.2f);
            }
        }else if(Command == 14){ //反応射撃
            if(getDataOnly != -1){
                if(isCrit)yield return new ActionData(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateZero });
                    else yield return new ActionData(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>());
                yield break;
            }
            if(isBurst){
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                TargetObj = AutoSelectTarget(ActorObj, 0);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    if(Target.getTarget() == Actor){
                        if(isCrit)Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateZero });
                            else Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>());
                    }
                }

                yield return new WaitForSeconds(0.1f);
                TargetObj = AutoSelectTarget(ActorObj, 1);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    if(Target.getTarget() == Actor){
                        if(isCrit)Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateZero });
                            else Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>());
                    }
                }

                yield return new WaitForSeconds(0.1f);
                TargetObj = AutoSelectTarget(ActorObj, 2);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    if(Target.getTarget() == Actor){
                        if(isCrit)Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>(){ Mods.DodgeRateZero });
                            else Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>());
                    }
                }
                yield return new WaitForSeconds(1f);
            }else{
                yield return null;
            }

        }else if(Command == 15){ //強化ヘッドショット
            if(getDataOnly != -1){
                if(isCrit)yield return new ActionData(Actor, Target, AttackDamage, 3f, HitRate, new List<Mods>());
                    else yield return new ActionData(Actor, Target, AttackDamage, 2f, HitRate, new List<Mods>());
                yield break;
            }
            if(isBurst){
                yield return null;
            }else{
                Debug.Log("ヘッドショット！");
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                if(isCrit)Attack(Actor, Target, AttackDamage, 3f, HitRate, new List<Mods>());
                    else Attack(Actor, Target, AttackDamage, 2f, HitRate, new List<Mods>());
                yield return new WaitForSeconds(1f);
            }

        }else if(Command == 16){ //とどめ

            int HitBuffCalc = 0;
            int DodBuffCalc = 0;
            foreach(int value in Target.HitRate){
                HitBuffCalc += value;
            }
            foreach(int value in Target.DodgeRate){
                DodBuffCalc += value;
            }

            if(getDataOnly != -1){
                if(isCrit){
                    if(HitBuffCalc < 0 || DodBuffCalc < 0){
                        yield return new ActionData(Actor, Target, AttackDamage, 3f, HitRate, new List<Mods>());
                    }else{
                        yield return new ActionData(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>());
                    }
                }else{
                    if(HitBuffCalc < 0 || DodBuffCalc < 0){
                        yield return new ActionData(Actor, Target, AttackDamage, 2f, HitRate, new List<Mods>());
                    }else{
                        yield return new ActionData(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>());
                    }
                }
                yield break;
            }

            if(isBurst){
                yield return null;
            }else{
                Debug.Log("とどめ！");
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                if(isCrit){
                    if(HitBuffCalc < 0 || DodBuffCalc < 0){
                        Attack(Actor, Target, AttackDamage, 3f, HitRate, new List<Mods>());
                    }else{
                        Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>());
                    }
                }else{
                    if(HitBuffCalc < 0 || DodBuffCalc < 0){
                        Attack(Actor, Target, AttackDamage, 2f, HitRate, new List<Mods>());
                    }else{
                        Attack(Actor, Target, AttackDamage, 1f, HitRate, new List<Mods>());
                    }
                }
                yield return new WaitForSeconds(1f);
            }

        }else if(Command == 1001){ //敵1通常攻撃
            if(isBurst){
                Target.setIsTargeted(true);
                yield return null;
            }else{
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                Attack(Actor, Target, AttackDamage, 1f, 80, new List<Mods>());
                yield return new WaitForSeconds(1.5f);
            }

        }else if(Command == 1002){ //敵2通常攻撃
            if(isBurst){
                Target.setIsTargeted(true);
                yield return null;
            }else{
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                Attack(Actor, Target, AttackDamage, 1f, 80, new List<Mods>());
                yield return new WaitForSeconds(1.5f);
            }
        }else if(Command == 1003){ //敵2グレネード
            if(isBurst){

                TargetObj = AutoSelectTarget(ActorObj, 0);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Target.setIsTargeted(true);
                }
                TargetObj = AutoSelectTarget(ActorObj, 1);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Target.setIsTargeted(true);
                }
                TargetObj = AutoSelectTarget(ActorObj, 2);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Target.setIsTargeted(true);
                }

                yield return null;
            }else{
                Debug.Log("グレネード！");
                yield return new WaitForSeconds(0.1f);
                ActorObj.GetComponent<Animator>().SetBool("Fire_Grenade", true);
                yield return new WaitForSeconds(0.7f);
                int dmg = 1;

                TargetObj = AutoSelectTarget(ActorObj, 0);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Attack(Actor, Target, 1+dmg, 1f, 100, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateZero });
                    dmg = 0;
                }

                yield return new WaitForSeconds(0.1f);
                TargetObj = AutoSelectTarget(ActorObj, 1);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Attack(Actor, Target, 1+dmg, 1f, 100, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateZero });
                    dmg = 0;
                }

                yield return new WaitForSeconds(0.1f);
                TargetObj = AutoSelectTarget(ActorObj, 2);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Attack(Actor, Target, 1+dmg, 1f, 100, new List<Mods>(){ Mods.DodgeRateDouble, Mods.MissRateZero });
                    dmg = 0;
                }

                yield return new WaitForSeconds(1f);
            }
        }else if(Command == 1004){ //敵3レーザー
            if(isBurst){

                TargetObj = AutoSelectTarget(ActorObj, 0);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Target.setIsTargeted(true);
                }
                TargetObj = AutoSelectTarget(ActorObj, 1);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Target.setIsTargeted(true);
                }
                TargetObj = AutoSelectTarget(ActorObj, 2);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Target.setIsTargeted(true);
                }

                yield return null;
            }else{
                Debug.Log("レーザー！");
                yield return new WaitForSeconds(0.1f);
                ActorObj.GetComponent<Animator>().SetBool("Fire_Special", true);
                yield return new WaitForSeconds(0.7f);

                TargetObj = AutoSelectTarget(ActorObj, 0);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Attack(Actor, Target, 2, 1f, 90, new List<Mods>(){ Mods.DodgeRateDouble });
                }

                yield return new WaitForSeconds(0.1f);
                TargetObj = AutoSelectTarget(ActorObj, 1);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Attack(Actor, Target, 2, 1f, 90, new List<Mods>(){ Mods.DodgeRateDouble });
                }

                yield return new WaitForSeconds(0.1f);
                TargetObj = AutoSelectTarget(ActorObj, 2);
                if(TargetObj != null){
                    Target = TargetObj.GetComponent<Character>();
                    Attack(Actor, Target, 2, 1f, 90, new List<Mods>(){ Mods.DodgeRateDouble });
                }

                yield return new WaitForSeconds(1f);
            }
        }else if(Command == 1005){ //敵2回避攻撃
            if(isBurst){
                Target.setIsTargeted(true);
                Actor.DodgeRateBuff(40, 1);
                Actor.GrazeBuff(1, 1);
                yield return null;
            }else{
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                Attack(Actor, Target, AttackDamage, 1f, 80, new List<Mods>());
                yield return new WaitForSeconds(1.5f);
            }
        }else if(Command == 1006){ //敵3盾攻撃
            if(isBurst){
                Target.setIsTargeted(true);
                Actor.ShieldingBuff(3, 1);
                yield return null;
            }else{
                yield return new WaitForSeconds(0.5f);
                ActorObj.GetComponent<Animator>().SetBool("Fire", true);
                Attack(Actor, Target, AttackDamage, 1f, 80, new List<Mods>());
                yield return new WaitForSeconds(1.5f);
            }
        }

    }

    public int ActionBias(GameManager_MainScene GM, Character actor, int ID){
        if(ID == 1){
            if(actor.getIsTargeted() == true){
                return 200;
            }else{
                return 0;
            }

        }else if(ID == 2){
            if(actor.getIsTargeted() == true){
                return 200;
            }else{
                return 0;
            }

        }else if(ID == 8){
            if(GM_I.GetItemDataList().Count(item => item.getSortNo() == actor.getNumber()) != 0){
                if(GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == actor.getNumber()).addActionA == 15 ||
                    GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == actor.getNumber()).addActionB == 15 ||
                    GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == actor.getNumber()).addActionC == 15){
                    return 0;
                }else{
                    return 100;
                }
            }else{
                return 100;
            }
        }else if(ID == 10){
            if(GM_I.GetItemDataList().Count(item => item.getSortNo() == actor.getNumber()+3) != 0){
                if(GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == actor.getNumber()+3).addActionA == 100 ||
                    GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == actor.getNumber()+3).addActionB == 100 ||
                    GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == actor.getNumber()+3).addActionC == 100){
                    return 100;
                }else{
                    return 0;
                }
            }else{
                return 0;
            }
        }

        return 100;
    }
}
