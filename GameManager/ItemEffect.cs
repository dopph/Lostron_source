using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Item {


    public void UseItem(int ID, Character Target, Item_Drag_Chara DropUI){
        if(ID == 302001){
            Target.Heal(2);
            DropUI.CallHealEffect();
        }else if(ID == 302002){
            Target.Heal(999);
            DropUI.CallHealEffect();
        }
    }

}
