using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager_ItemManage : MonoBehaviour
{
    [SerializeField] Entity_sheet1 ItemDBsheet;
    [SerializeField] List<int> AddItemList;
    [SerializeField] bool BringItem;
    private List<ItemDB> ItemDB = new List<ItemDB>();
    private List<Item> Items = new List<Item>();
    public bool isDraggingHeal;

    void Awake() {

        //各シートのデータを順に取り出す
        foreach (Entity_sheet1.Sheet ItemSheet in ItemDBsheet.sheets) {
            //各項目のデータを順に取り出す
            foreach (Entity_sheet1.Param ItemParam in ItemSheet.list) {
                if(ItemParam.Name != "")ItemDB.Add(new ItemDB(ItemParam.ID, ItemParam.Type, ItemParam.Name, ItemParam.Explain, ItemParam.Rarity, ItemParam.Address, ItemParam.AnimName, (ItemDB.Attributes)Enum.Parse(typeof(ItemDB.Attributes), ItemParam.Attribute), ItemParam.Damage, ItemParam.Hit, ItemParam.AddActionA, ItemParam.AddActionB, ItemParam.AddActionC));
            }
        }

        int Num = 0;
        foreach(int Value in AddItemList){
            addItem(Value, Num);
            Num++;
        }
        
        if(StartItem.isContinue == false){
            if(BringItem && StartItem.StartItemID != 0){
                addItem(StartItem.StartItemID, 6);
            }
        }else{
            Items = ES3.Load<List<Item>>("item");
        }

    }

    public List<Item> GetItemDataList(){
        return Items;
    }

    public void ClearLoot(){
        Items.RemoveAll(item => item.getSortNo() >= 100);
    }

    public void DeleteItem(int Num){
        Items.RemoveAll(item => item.getSortNo() == Num);
    }

    public void addItem(int ID, int sortNo){
        Items.Add(ItemDB.First(item => item.ID == ID).generateItem(0));
        Items[(Items.Count)-1].sortItem(sortNo);
    }

    public void setIsDragging(bool Value){
        isDraggingHeal = Value;
    }
    public bool getIsDragging(){
        return isDraggingHeal;
    }
}
