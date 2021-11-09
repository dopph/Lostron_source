using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public partial class Item{

    //public GameManager_MainScene GameManager;
    private int ID;
    private int sortNo;
    private int Type;
    private string SpriteAddress;
    private string Name;
    private string Explain;
    private int Rarity;
    public int rarity{ get{ return Rarity; } set{ Rarity = value;} }
    private string AnimName;

    public Attributes Attribute;
    public enum Attributes{
        None,
        Physical,
        Energy,
        Heat,
        Heal
    }
    private int Damage;
    private int Hit;
    private int AddActionA;
    public int addActionA{ get{ return AddActionA; } set{ AddActionA = value;} }
    private int AddActionB;
    public int addActionB{ get{ return AddActionB; } set{ AddActionB = value;} }
    private int AddActionC;
    public int addActionC{ get{ return AddActionC; } set{ AddActionC = value;} }

    private Sprite mySprite = null;

    public Item(int ID, int Type, string Name, string Explain, int Rarity, string SpriteAddress, string AnimName, Attributes Attribute, int Damage, int Hit, int AddActionA, int AddActionB, int AddActionC){
        this.ID = ID;
        this.Type = Type;
        this.Name = Name;
        this.Explain = Explain;
        this.Rarity = Rarity;
        this.SpriteAddress = SpriteAddress;
        this.AnimName = AnimName;
        this.Attribute = Attribute;
        this.Damage = Damage;
        this.Hit = Hit;
        this.AddActionA = AddActionA;
        this.AddActionB = AddActionB;
        this.AddActionC = AddActionC;
        if(SpriteAddress != "")LoadSprite();
    }

    public void LoadSprite(){
        // Addressablesによる読み込み
        Addressables.LoadAssetAsync<Sprite>(SpriteAddress).Completed += handle =>
        {
            // ロードに成功した場合の処理をここに
            mySprite = handle.Result;
        };

    }

    public void sortItem(int no){
        this.sortNo = no;
    }

    public int getID(){
        return this.ID;
    }
    public Sprite getSprite(){
        return this.mySprite;
    }
    public int getItemType(){
        return this.Type;
    }
    public int getSortNo(){
        return this.sortNo;
    }
    public string getName(){
        return this.Name;
    }
    public string getExplain(){
        return this.Explain;
    }
    public string getAnimName(){
        return this.AnimName;
    }
    public int getDamage(){
        return this.Damage;
    }
    public int getHit(){
        return this.Hit;
    }
    public Attributes getAttribute(){
        return this.Attribute;
    }
}

public class ItemDB{

    public int ID;
    private int Type;
    private string SpriteAddress;
    private string Name;
    private string Explain;
    private int Rarity;
    private string AnimName;

    public Attributes Attribute;
    public enum Attributes{
        None,
        Physical,
        Energy,
        Heat,
        Heal
    }
    private int Damage;
    private int Hit;
    private int AddActionA;
    public int addActionA{ get{ return AddActionA; } set{ AddActionA = value;} }
    private int AddActionB;
    public int addActionB{ get{ return AddActionB; } set{ AddActionB = value;} }
    private int AddActionC;
    public int addActionC{ get{ return AddActionC; } set{ AddActionC = value;} }

    public ItemDB(int ID, int Type, string Name, string Explain, int Rarity, string SpriteAddress, string AnimName, Attributes Attribute, int Damage, int Hit, int AddActionA, int AddActionB, int AddActionC){
        this.ID = ID;
        this.Type = Type;
        this.Name = Name;
        this.Explain = Explain;
        this.Rarity = Rarity;
        this.SpriteAddress = SpriteAddress;
        this.AnimName = AnimName;
        this.Attribute = Attribute;
        this.Damage = Damage;
        this.Hit = Hit;
        this.AddActionA = AddActionA;
        this.AddActionB = AddActionB;
        this.AddActionC = AddActionC;
    }

    public Item generateItem(int owner){
        Item ItemData = new Item(this.ID, this.Type, this.Name, this.Explain, this.Rarity, this.SpriteAddress, this.AnimName, (Item.Attributes)this.Attribute, this.Damage, this.Hit, this.AddActionA, this.AddActionB, this.AddActionC);
        return ItemData;
    }
    
}

public class ActionRelationalDB{

    public int ItemType;
    public int ActionID;

    public ActionRelationalDB(int ItemType, int ActionID){
        this.ItemType = ItemType;
        this.ActionID = ActionID;
    }

}