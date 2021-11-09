using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog_Button : MonoBehaviour
{
    public int Value;
    public CommonDialog Parent;

    public void SendValue(){
        Parent.SetValue(this.Value);
    }
}
