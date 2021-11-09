using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCard : MonoBehaviour
{
    public Vector3 movePos;
    public Sprite openSprite;
    private float spd;
    private float _progress;

    // Start is called before the first frame update
    void Start()
    {
        movePos = this.transform.localPosition;
        spd = 9f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, movePos, spd * Time.unscaledDeltaTime);
        spd = spd * 0.997f;
        if(spd < 3.5f)spd = 3.5f;
    }

    public void posChange(int x){
        movePos = new Vector3(x, movePos.y, 0);
        spd = 9f;
    }

    public void posAdd(int x, int y = 0){
        movePos += new Vector3(x, y, 0);
        spd = 9f;
    } 
}
