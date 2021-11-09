using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item_Drag_Chara : MonoBehaviour, IDropHandler, IPointerExitHandler
{
    private GameManager_MainScene GM;
    private GameManager_ItemManage GM_I;
    private Transform canvasTran;
    private GameObject draggingObject;
    private Item itemData;
    private int CharaNo;
    private GameObject HealMark;
    private GameObject HealEffectObject;

    public void setNumber(int Value){
        CharaNo = Value;
    }
    
    void Start()
    {
        canvasTran = transform.parent.parent.parent;
        GM = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MainScene>();
        GM_I = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_ItemManage>();
        HealMark = transform.Find("HealObject_01/Icon_Health").gameObject;
        HealEffectObject = transform.Find("HealObject_01/HealEffectObject").gameObject;
    }

    void Update(){
        HealMark.SetActive(GM_I.getIsDragging());
    }

    public void CallHealEffect(){
        HealEffectObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(pointerEventData.pointerDrag == null || pointerEventData.pointerDrag.GetComponent<Item_Drag>().nowSprite.name == "Item_0000") return;
    }

    public void OnDrop(PointerEventData pointerEventData)
    {
        int UseItemNo = pointerEventData.pointerDrag.GetComponent<Item_Drag>().InventoryNo;

        if(pointerEventData.pointerDrag == null || pointerEventData.pointerDrag.GetComponent<Item_Drag>().nowSprite.name == "Item_0000") return;
        if(GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == UseItemNo).getItemType() != 4)return;

        GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == UseItemNo).UseItem(GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == UseItemNo).getID(), GM.Players[CharaNo].GetComponent<Character>(), this);
        GM_I.DeleteItem(UseItemNo);
        pointerEventData.pointerDrag.GetComponent<Item_Drag>().imageUpdate();
    }

}

