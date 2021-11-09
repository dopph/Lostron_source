using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CommandWindow : MonoBehaviour
{

    public GameManager_MainScene GM;
    public GameManager_ItemManage GM_I;
    
    [SerializeField] ActionsDB ActionDBsheet;
    [SerializeField] GameObject Card;
    [SerializeField] Text NameText;
    [SerializeField] Text ExplainText;
    [SerializeField] List<GameObject> Buttons;
    private List<ActionCard> ActionCardDB = new List<ActionCard>();
    public int Command;
    public int TargetType;
    public GameObject Target;
    public GameObject TargetEnemy;
    public GameObject TargetAlly;
    private List<int> IdeaPool;
    private List<int> Ideas;
    private List<int> IdeaTargetType;
    public Character Actor;
    public int currentCard = 0;
    private List<GameObject> Cards;
    public bool isCrit;
    private int MainEquipAddActionA;
    private int MainEquipAddActionB;
    private int MainEquipAddActionC;
    private int SubEquipAddActionA;
    private int SubEquipAddActionB;
    private int SubEquipAddActionC;
    private bool isPistol;
    void Awake(){
        Command = -1;
    }

    void Start(){
        //Debug.Log(GM_I.getActionRelationalDB().Count(d => (d.ItemType == MainEquipID)));
        //Debug.Log(SubEquipID);

        //各シートのデータを順に取り出す
        foreach (ActionsDB.Sheet ActionSheet in ActionDBsheet.sheets) {
            //各項目のデータを順に取り出す
            foreach (ActionsDB.Param ActionParam in ActionSheet.list) { 
                if( ActionParam.ID == 2 ||
                    ActionParam.ID == 3 ||
                    ActionParam.ID == 4 ||
                    ActionParam.ID == 5 ||
                    ActionParam.ID == 8 ||
                    ActionParam.ID == 11 ||
                    ActionParam.ID == 12 ||
                    ActionParam.ID == MainEquipAddActionA ||
                    ActionParam.ID == MainEquipAddActionB ||
                    ActionParam.ID == MainEquipAddActionC ||
                    ActionParam.ID == SubEquipAddActionA ||
                    ActionParam.ID == SubEquipAddActionB ||
                    ActionParam.ID == SubEquipAddActionC ||
                    ( ActionParam.ID == 10 && isPistol )){
                        ActionCardDB.Add(new ActionCard(ActionParam.ID, ActionParam.Name, ActionParam.Explain, ActionParam.CritExplain, ActionParam.Address, ActionParam.TargetType));
                    }
            }
        }

        int pickValue = 3 + Actor.IdeaPlus_;
        if(Actor.getPassive() == GameManager_MainScene.Passives.AIvisor && Actor.getIsTargeted() == false)pickValue++;
        Ideas = new List<int>();
        int pickNum;
        int BiasSum;
        int n;

        List<int> IdeaPool = new List<int>();
        List<int> IdeaBias = new List<int>();
        foreach (ActionCard CardList in ActionCardDB) {
            IdeaPool.Add( CardList.getID() );
            IdeaBias.Add( GM.ActionBias(GM, Actor, CardList.getID()) );
        }

        if(pickValue > IdeaPool.Count)pickValue = IdeaPool.Count;

        if(pickValue > 0){
            for(int i=0; i < pickValue; i++){

                BiasSum = IdeaBias.Sum();
                pickNum = Random.Range(0, BiasSum)+1;
                n = 0;
                foreach (int Bias in IdeaBias) {
                    pickNum -= Bias;
                    if(pickNum <= 0){
                        Ideas.Add(IdeaPool[n]);
                        IdeaPool.RemoveAt(n);
                        IdeaBias.RemoveAt(n);
                        break;
                    }
                    n++;
                }
            }
            Cards = new List<GameObject>();
            currentCard = 0;
            StartCoroutine(StartSelect());
        }else{
            Command = 0;
        }

        TargetEnemy = GM.AutoSelectTarget(Actor.transform.gameObject, -1, false);
        TargetAlly = GM.AutoSelectTarget(Actor.transform.gameObject, -1, true);
    }

    private IEnumerator StartSelect(){

        GameObject c;
        
        for(int i=0; i < Ideas.Count; i++){

            c = Instantiate(Card);
            c.transform.SetParent(transform.Find("Canvas").transform, false);
            c.GetComponent<RectTransform>().localPosition += new Vector3(-32*i, 0, 0);
            Cards.Add(c);
            LoadSprite(i, Ideas[i]);

        }

        yield return new WaitForSecondsRealtime(0.3f);
        PickCard(0);

        transform.Find("Canvas/AnimObject/Common_Button_Ok").GetComponent<Button>().enabled = true;
        transform.Find("Canvas/AnimObject/Common_Button_Next").GetComponent<Button>().enabled = true;

    }

    public void setEquipData(int MainEquipAddActionA, int MainEquipAddActionB, int MainEquipAddActionC, int SubEquipAddActionA, int SubEquipAddActionB, int SubEquipAddActionC, bool isPistol){
        this.MainEquipAddActionA = MainEquipAddActionA;
        this.MainEquipAddActionB = MainEquipAddActionB;
        this.MainEquipAddActionC = MainEquipAddActionC;
        this.SubEquipAddActionA = SubEquipAddActionA;
        this.SubEquipAddActionB = SubEquipAddActionB;
        this.SubEquipAddActionC = SubEquipAddActionC;
        this.isPistol = isPistol;
    }

    void LoadSprite(int num, int i){

        Sprite cardSprite;
        string Address = ActionCardDB.First(c => c.getID() == i).getAddress();
        // Addressablesによる読み込み
        Addressables.LoadAssetAsync<Sprite>(Address).Completed += handle =>{
            // ロードに成功した場合の処理をここに
            cardSprite = handle.Result;
            Cards[num].GetComponent<CommandCard>().openSprite = cardSprite;
        };

    }

    public void PickCard(int num){

        Cards[num].GetComponent<CommandCard>().posChange(0);
        Cards[num].GetComponent<Image>().sprite = Cards[num].GetComponent<CommandCard>().openSprite;
        NameText.text = ActionCardDB.First(ca => ca.getID() == Ideas[num]).getName();
        if(currentCard != Ideas.Count-1)ExplainText.text = ActionCardDB.First(ca => ca.getID() == Ideas[num]).getExplain();
            else ExplainText.text = ActionCardDB.First(ca => ca.getID() == Ideas[num]).getCritExplain();
        TargetType = ActionCardDB.First(ca => ca.getID() == Ideas[num]).getTargetType();
        
        ActionData actionData0 = GM.getActionData(Ideas[num], Actor.transform.gameObject, false, 0);
        ActionData actionData1 = GM.getActionData(Ideas[num], Actor.transform.gameObject, false, 1);
        ActionData actionData2 = GM.getActionData(Ideas[num], Actor.transform.gameObject, false, 2);

        GameObject TargetSet = transform.Find("Canvas/AnimObject/TargetSet").gameObject;
        if(actionData0 != null)TargetSet.transform.Find("Common_Button_04/Text").GetComponent<Text>().text = (actionData0.FinalHitRate.ToString() + "％");
            else TargetSet.transform.Find("Common_Button_04/Text").GetComponent<Text>().text = "";
        if(actionData1 != null)TargetSet.transform.Find("Common_Button_05/Text").GetComponent<Text>().text = (actionData1.FinalHitRate.ToString() + "％");
            else TargetSet.transform.Find("Common_Button_05/Text").GetComponent<Text>().text = "";
        if(actionData2 != null)TargetSet.transform.Find("Common_Button_06/Text").GetComponent<Text>().text = (actionData2.FinalHitRate.ToString() + "％");
            else TargetSet.transform.Find("Common_Button_06/Text").GetComponent<Text>().text = "";

        DecideTarget();

    }

    public void skipCard(){

        currentCard++;
        int num = 0;
        foreach(GameObject card in Cards){
            if(num == currentCard){
                PickCard(num);
            }else if(num < currentCard){
                card.GetComponent<CommandCard>().posChange(280);
            }else{
                card.GetComponent<CommandCard>().posAdd(32);
            }
            num++;
        }
        if(currentCard >= Ideas.Count){
            Command = 0;
        }

    }

    public void DecideCard(){
        Command = Ideas[currentCard];
        if(currentCard == Ideas.Count-1)isCrit = true;
            else isCrit = false;

        if(TargetType == 1){
            this.Target = TargetEnemy;
        }else if(TargetType == -1){
            this.Target = TargetAlly;
        }else{
            this.Target = Actor.transform.gameObject;
        }
    }

    public void DecideTarget(){
        if(TargetType == 1){
            this.Target = TargetEnemy;
        }else if(TargetType == -1){
            this.Target = TargetAlly;
        }else{
            this.Target = Actor.transform.gameObject;
        }
        HighlightUpdate();
    }

    public void HighlightUpdate(){
        setHighlight(GM.Players[0], false);
        setHighlight(GM.Players[1], false);
        setHighlight(GM.Players[2], false);
        setHighlight(GM.Enemies[0], false);
        setHighlight(GM.Enemies[1], false);
        setHighlight(GM.Enemies[2], false);
        setHighlight(Actor.transform.gameObject, true);
        if(TargetType == 1){
            setHighlight(TargetEnemy, true);
        }else if(TargetType == -1){
            setHighlight(TargetAlly, true);
        }else if(TargetType == 2){
            setHighlight(GM.Enemies[0], true);
            setHighlight(GM.Enemies[1], true);
            setHighlight(GM.Enemies[2], true);
        }else if(TargetType == -2){
            setHighlight(GM.Players[0], true);
            setHighlight(GM.Players[1], true);
            setHighlight(GM.Players[2], true);
        }
    }

    void OnDestroy(){
        setHighlight(GM.Players[0], false);
        setHighlight(GM.Players[1], false);
        setHighlight(GM.Players[2], false);
        setHighlight(GM.Enemies[0], false);
        setHighlight(GM.Enemies[1], false);
        setHighlight(GM.Enemies[2], false);
    }

    public void setHighlight(GameObject Chara, bool value){
        if(Chara.GetComponent<Character>().isDown == false){
            foreach(SpriteRenderer SR in Chara.transform.Find("SpriteSet").GetComponentsInChildren<SpriteRenderer>() ){
                if(value == true)SR.sortingLayerName = "Unhide Object";
                if(value == false)SR.sortingLayerName = "Default";
            }
        }
    }
}

