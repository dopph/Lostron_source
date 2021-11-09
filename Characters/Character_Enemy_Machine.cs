using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Enemy_Machine : Character
{

    private Animator animator;
    [SerializeField] List<int> ActionList;
    [SerializeField] int ATK;
    public int defaultHP;

    void Start(){
        StatusReset(true);
        isDown = false;
        animator = GetComponent<Animator>();
        setHP(defaultHP, defaultHP);
    }

    void FixedUpdate()
    {
        base.FU();
    }

    public override int getATK(){
        return ATK;
    }

    public void StopShotAnim(){
        GetComponent<Animator>().SetBool("Fire", false);
    }

    public List<int> getActionList(){
        return ActionList;
    }

    public override void TakeDamage(int Damage, bool motion = true){
        base.TakeDamage(Damage, motion);
        if(Damage >= 0 && motion == true)animator.SetBool("Damage", true);
    }
    public override void Down(){
        base.Down();
        animator.SetBool("Die", true);
    }

    public override GameManager_MainScene.Passives getPassive(){
        return GameManager_MainScene.Passives.None;
    }

}
