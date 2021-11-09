using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class NewBehaviourScript : MonoBehaviour
{
    private Animator anim;
    private Character Parent;

    void Start(){
        anim = GetComponent<Animator>();
        Parent = transform.parent.parent.parent.GetComponent<Character>();
    }

    void Update(){
        float Percent = 1 - ((float)Parent.getHP() / (float)Parent.getMaxHP());
        anim.Play("HealthBar_100", 0, (float)Percent);
    }
}
