using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallCharacterUIBody : MonoBehaviour
{

    private GameManager_MainScene GM;
    public GameObject[] UIPlayers = new GameObject[3];
    public GameObject[] HealthBar = new GameObject[3];
    // Start is called before the first frame update
    void Start()
    {
        GameObject init;
        GM = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MainScene>();

        if(GM.Players[0].GetComponent<Character>().isDown == false){
            init = Instantiate(GM.Players[0].GetComponent<Character_Player>().getUIBody());
            init.transform.Translate (77.0f, 25.0f, 0.0f);
            init.transform.SetParent(this.transform, false);
            init.transform.SetSiblingIndex(0); 
            init.GetComponent<Item_Drag_Chara>().setNumber(0);
            UIPlayers[0] = init;
        }
        if(GM.Players[1].GetComponent<Character>().isDown == false){
            init = Instantiate(GM.Players[1].GetComponent<Character_Player>().getUIBody());
            init.transform.Translate (0.0f, 25.0f, 0.0f);
            init.transform.SetParent(this.transform, false);
            init.transform.SetSiblingIndex(0); 
            init.GetComponent<Item_Drag_Chara>().setNumber(1);
            UIPlayers[1] = init;
        }
        if(GM.Players[2].GetComponent<Character>().isDown == false){
            init = Instantiate(GM.Players[2].GetComponent<Character_Player>().getUIBody());
            init.transform.Translate (-77.0f, 25.0f, 0.0f);
            init.transform.SetParent(this.transform, false);
            init.transform.SetSiblingIndex(0);
            init.GetComponent<Item_Drag_Chara>().setNumber(2);
            UIPlayers[2] = init;
        }
    }

}
