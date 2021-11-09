using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item_Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameManager_MainScene GM;
    private GameManager_ItemManage GM_I;
    private Transform canvasTran;
    private GameObject draggingObject;
    private Sprite emptySprite;
    public GameObject ItemImage;
    public Image iconImage;
    public Sprite nowSprite;
    public int InventoryNo;
    [SerializeField] int ItemType;

    [SerializeField] GameObject EmptyFrame;
    [SerializeField] GameObject CommonFrame;
    [SerializeField] GameObject RareFrame;
    [SerializeField] GameObject EpicFrame;
    private GameObject ItemExplain;
    private Item ItemData;
    private bool DestroyHealIcon;

    void Start()
    {
        emptySprite = nowSprite;
        canvasTran = transform.parent.parent.parent;
        GM = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_MainScene>();
        GM_I = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_ItemManage>();
        imageUpdate();
        ItemExplain = transform.parent.parent.parent.transform.Find("Object_Right").gameObject;
    }

    public void imageUpdate(){

        List<Item> Items = GM_I.GetItemDataList();
        Item matchItem = Items.FirstOrDefault(item => item.getSortNo() == InventoryNo);
        if(matchItem != null){
            setImage(matchItem.getSprite());

        }else{
            setImage(emptySprite);
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
        
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if(nowSprite == null || nowSprite.name == "Item_0000")return;
        if (pointerEventData.button != PointerEventData.InputButton.Left) return;
        CreateDragObject();
        draggingObject.transform.position = pointerEventData.position;
        if(GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == pointerEventData.pointerDrag.GetComponent<Item_Drag>().InventoryNo).getAttribute() == Item.Attributes.Heal){
            GM_I.setIsDragging(true);
        }
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        if(nowSprite == null || nowSprite.name == "Item_0000")return;
        if (pointerEventData.button != PointerEventData.InputButton.Left) return;
        draggingObject.transform.position = pointerEventData.position;
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {   
        int n = pointerEventData.pointerDrag.GetComponent<Item_Drag>().InventoryNo;
        
        if(GM_I.getIsDragging()){
            GM_I.setIsDragging(false);
        }
        Destroy(draggingObject);
    }

    // ドラッグオブジェクト作成
    private void CreateDragObject()
    {
        draggingObject = new GameObject("Dragging Object");
        draggingObject.transform.SetParent(canvasTran);
        draggingObject.transform.SetAsLastSibling();
        draggingObject.transform.localScale = Vector3.one;
        draggingObject.layer = 5;

        // レイキャストがブロックされないように
        CanvasGroup canvasGroup = draggingObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        Image draggingImage = draggingObject.AddComponent<Image>();
        Image sourceImage = ItemImage.GetComponent<Image>();

        draggingImage.sprite = sourceImage.sprite;
        draggingImage.rectTransform.sizeDelta = sourceImage.rectTransform.sizeDelta;
        draggingImage.color = sourceImage.color;
        draggingImage.material = sourceImage.material;

    }

    public Image getImage(){
        return ItemImage.GetComponent<Image>();
    }
    public void setImage(Sprite sprite){
        ItemImage.GetComponent<Image>().sprite = sprite;
        nowSprite = sprite;
    }
    public void removeImage(Sprite sprite){
        ItemImage.GetComponent<Image>().sprite = sprite;
        nowSprite = sprite;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Item ItemData;

        if(pointerEventData.pointerDrag == null || pointerEventData.pointerDrag.GetComponent<Item_Drag>().nowSprite.name == "Item_0000"){
            if(this.nowSprite.name != "Item_0000"){
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
        }else{
            if(ItemType == 0 || GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == pointerEventData.pointerDrag.GetComponent<Item_Drag>().InventoryNo).getItemType() == ItemType){
                Image droppedImage = pointerEventData.pointerDrag.GetComponent<Item_Drag>().getImage();
                iconImage.sprite = droppedImage.sprite;
                iconImage.color = Vector4.one * 0.6f;
            }
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(pointerEventData.pointerDrag == null || pointerEventData.pointerDrag.GetComponent<Item_Drag>().nowSprite.name == "Item_0000") return;
        iconImage.color = Vector4.one;
        imageUpdate();
    }

    public void OnDrop(PointerEventData pointerEventData)
    {
        Item swapItemBuffa;
        Item droppedItemBuffa;
        if(pointerEventData.pointerDrag == null || pointerEventData.pointerDrag.GetComponent<Item_Drag>().nowSprite.name == "Item_0000") return;
        if (pointerEventData.button != PointerEventData.InputButton.Left) return;
        if(ItemType != 0 && GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == pointerEventData.pointerDrag.GetComponent<Item_Drag>().InventoryNo).getItemType() != ItemType)return;

        List<Item> Items = GM_I.GetItemDataList();
        int droppedItemNo = pointerEventData.pointerDrag.GetComponent<Item_Drag>().InventoryNo;
        int swapItemNo = this.InventoryNo;

        swapItemBuffa = Items.FirstOrDefault(item => item.getSortNo() == swapItemNo);
        droppedItemBuffa = Items.FirstOrDefault(item => item.getSortNo() == droppedItemNo);
    
        if(swapItemBuffa != null){
            if(swapItemBuffa.getItemType() != droppedItemBuffa.getItemType() && (swapItemNo <= 5 || droppedItemNo <= 5) ){
                iconImage.color = Vector4.one;
                return;
            }
            swapItemBuffa.sortItem(droppedItemNo);
        }else{
            if(droppedItemNo <= 2)return;
        }
        droppedItemBuffa.sortItem(swapItemNo);

        iconImage.color = Vector4.one;
        imageUpdate();
        pointerEventData.pointerDrag.GetComponent<Item_Drag>().imageUpdate();
    }

    void Update(){
        if(Input.GetMouseButton(0) == false)imageUpdate();
    }
}
