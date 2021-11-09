using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwap : MonoBehaviour
{
    private GameManager_MainScene GM;
    private Character Parent;
    [SerializeField] int num;

    public void Start(){
        GM = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MainScene>();
        this.Parent = GM.Players[num].GetComponent<Character>();
        if(GM.Players[num].GetComponent<Character>().isDown == true && GM.Players[num+1].GetComponent<Character>().isDown == true)this.transform.gameObject.SetActive(false);
    }

    public void SwapCharacterButton(){

        GameObject window = transform.parent.parent.gameObject;
        GM.SwapCharacter(num);

        CallCharacterUIBody CUI = window.GetComponent<CallCharacterUIBody>();
        if( CUI.UIPlayers[num] != null   )CUI.UIPlayers[num].GetComponent<RectTransform>().localPosition += new Vector3(-77f, 0, 0);
        if( CUI.UIPlayers[num+1] != null )CUI.UIPlayers[num+1].GetComponent<RectTransform>().localPosition += new Vector3(77f, 0, 0);

        if( CUI.UIPlayers[num] != null   )CUI.HealthBar[num].GetComponent<RectTransform>().localPosition += new Vector3(-77f, 0, 0);
        if( CUI.UIPlayers[num+1] != null )CUI.HealthBar[num+1].GetComponent<RectTransform>().localPosition += new Vector3(77f, 0, 0);
        
        GameObject CharaStock = CUI.UIPlayers[num+1];
        CUI.UIPlayers[num+1] = CUI.UIPlayers[num];
        CUI.UIPlayers[num] = CharaStock;

        CharaStock = CUI.HealthBar[num+1];
        CUI.HealthBar[num+1] = CUI.HealthBar[num];
        CUI.HealthBar[num] = CharaStock;

    }

}
