using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class LossyScale : MonoBehaviour
{
    public Vector2 defaultScale = Vector2.zero;

    void Start () 
    {
        defaultScale = transform.lossyScale;
    }
    
    void Update () 
    {
        Vector3 lossScale = transform.lossyScale;
        Vector3 localScale = transform.localScale;

        transform.localScale = new Vector2(
                defaultScale.x,
                defaultScale.y
        );
    }
}
