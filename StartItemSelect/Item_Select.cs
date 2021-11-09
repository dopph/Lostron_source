using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Item_Select : MonoBehaviour
{
    private StartItem GM;
    private GameManager_ItemManage GM_I;
    private GameObject ItemExplain;
    private Sprite emptySprite;
    public int InventoryNo;
    public GameObject ItemImage;
    public Sprite nowSprite;

    private GameObject EmptyFrame;
    private GameObject CommonFrame;
    private GameObject RareFrame;
    private GameObject EpicFrame;

    void Start()
    {
        EmptyFrame = transform.Find("Base_01").gameObject;
        CommonFrame = transform.Find("Base_02").gameObject;
        RareFrame = transform.Find("Base_03").gameObject;
        EpicFrame = transform.Find("Base_04").gameObject;
        emptySprite = nowSprite;
        GM = transform.root.Find("Result_Inventory_StartItem").gameObject.GetComponent<StartItem>();
        GM_I = transform.root.Find("Result_Inventory_StartItem").gameObject.GetComponent<GameManager_ItemManage>();
        ItemExplain = transform.parent.parent.parent.transform.Find("Object_Right").gameObject;
        imageUpdate();
    }

    public void imageUpdate(){

        List<Item> Items = GM_I.GetItemDataList();
        Item matchItem = Items.FirstOrDefault(item => item.getSortNo() == InventoryNo);

        if(matchItem != null){
            StartCoroutine(setImage(matchItem));
        }

    }

    private IEnumerator setImage(Item matchItem){
        while(true){
            ItemImage.GetComponent<Image>().sprite = matchItem.getSprite();
            if( ItemImage.GetComponent<Image>().sprite != null){
                nowSprite = ItemImage.GetComponent<Image>().sprite;
                break;
            }else{
                ItemImage.GetComponent<Image>().sprite = emptySprite;
            }
            yield return null; 
        }

        if(nowSprite == null || nowSprite.name == "Item_0000"){
            EmptyFrame.SetActive (true);
            CommonFrame.SetActive (false);
            RareFrame.SetActive (false);
            EpicFrame.SetActive (false);
        }else{
            EmptyFrame.SetActive (false);
            CommonFrame.SetActive ((matchItem.rarity == 0)?true:false);
            RareFrame.SetActive ((matchItem.rarity == 1)?true:false);
            EpicFrame.SetActive ((matchItem.rarity == 2)?true:false);
        }
        yield return null; 
    }

    public void SelectItem(){

        Item ItemData;

        if(this.nowSprite.name != "Item_0000"){

            List<Item> Items = GM_I.GetItemDataList();
            StartItem.StartItemID = Items.FirstOrDefault(item => item.getSortNo() == InventoryNo).getID();
            Debug.Log(StartItem.StartItemID);

            ItemExplain.transform.Find("Item/Image").gameObject.GetComponent<Image>().sprite = this.nowSprite;
            ItemExplain.transform.Find("Text_ItemName").gameObject.GetComponent<Text>().text = GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == InventoryNo).getName();
            ItemExplain.transform.Find("TextSet/Text_ItemText").gameObject.GetComponent<Text>().text = GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == InventoryNo).getExplain();
            if(GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == InventoryNo).getAttribute() != Item.Attributes.None && GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == InventoryNo).getAttribute() != Item.Attributes.Heal){
                ItemExplain.transform.Find("TextSet/WeaponTextSet").gameObject.SetActive (true);
                ItemData = GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == InventoryNo);
                ItemExplain.transform.Find("TextSet/WeaponTextSet/Text_Damage").gameObject.GetComponent<Text>().text = "攻撃力："+(ItemData.getDamage());
                ItemExplain.transform.Find("TextSet/WeaponTextSet/Text_HitRate").gameObject.GetComponent<Text>().text = "命中率："+(ItemData.getHit());
                if(ItemData.addActionA != 0){
                    ItemExplain.transform.Find("TextSet/AddActionSet_01").gameObject.SetActive (true);
                    ItemExplain.transform.Find("TextSet/AddActionSet_01/Text_SpecialText").gameObject.GetComponent<Text>().text = GM.actionCardDB.First(act=> act.getID() == ItemData.addActionA).getName() + "：" + GM.actionCardDB.First(act=> act.getID() == ItemData.addActionA).getExplain();
                    ItemExplain.transform.Find("TextSet/AddActionSet_01/Image_BattleIcon").gameObject.GetComponent<Image>().sprite = GM.actionCardDB.First(act=> act.getID() == ItemData.addActionA).cardSprite;
                }else{
                    ItemExplain.transform.Find("TextSet/AddActionSet_01").gameObject.SetActive (false);
                }

                if(ItemData.addActionB != 0){
                    ItemExplain.transform.Find("TextSet/AddActionSet_02").gameObject.SetActive (true);
                    ItemExplain.transform.Find("TextSet/AddActionSet_02/Text_SpecialText").gameObject.GetComponent<Text>().text = GM.actionCardDB.First(act=> act.getID() == ItemData.addActionB).getName() + "：" + GM.actionCardDB.First(act=> act.getID() == ItemData.addActionB).getExplain();
                    ItemExplain.transform.Find("TextSet/AddActionSet_02/Image_BattleIcon").gameObject.GetComponent<Image>().sprite = GM.actionCardDB.First(act=> act.getID() == ItemData.addActionB).cardSprite;
                }else{
                    ItemExplain.transform.Find("TextSet/AddActionSet_02").gameObject.SetActive (false);
                }

            }else{
                ItemExplain.transform.Find("TextSet/WeaponTextSet").gameObject.SetActive (false);
                ItemExplain.transform.Find("TextSet/AddActionSet_01").gameObject.SetActive (false);
                ItemExplain.transform.Find("TextSet/AddActionSet_02").gameObject.SetActive (false);
            }
        }
    }

}
