using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Dummy : Character
{
    void Start(){
        StatusReset(true);
        isDown = true;
    }

    public override void TakeDamage(int Damage, bool motion = true){
    }

}
