using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager_MainScene : MonoBehaviour
{

    [SerializeField] GameObject Menu_UI;
    [SerializeField] GameObject CommandWindow;
    [SerializeField] bool isDebug;
    [SerializeField] GameObject GameOverWindow;
    [SerializeField] GameObject LootButton;
    [SerializeField] GameObject InventoryButton;
    [SerializeField] GameObject MapButton;
    [SerializeField] ActionsDB ActionDBsheet;
    private GameManager_ItemManage GM_I;
    private GameManager_Event GM_E;
    private GameManager_MapManage GM_M;
    private List<ActionCard> ActionCardDB = new List<ActionCard>();
    public List<ActionCard> actionCardDB{
        get{return ActionCardDB;} set{ActionCardDB = value;}
    }
    public GameObject Dummy_Character;
    public GameObject[] Dummys = new GameObject[6];
    public GameObject[] Players = new GameObject[3];
    public GameObject[] Enemies = new GameObject[3];
    public List<GameObject> AllObject = new List<GameObject>();
    
    public Phases Phase;
    public enum Phases{
        IDLE,
        IN_BATTLE,
        OVER
    }

    public void Start()
    {

        GameObject obj;
        
        GM_I = GetComponent<GameManager_ItemManage>();
        GM_E = GetComponent<GameManager_Event>();
        GM_M = GetComponent<GameManager_MapManage>();

        foreach (ActionsDB.Sheet ActionSheet in ActionDBsheet.sheets) {
            //各項目のデータを順に取り出す
            foreach (ActionsDB.Param ActionParam in ActionSheet.list) { 
                ActionCardDB.Add(new ActionCard(ActionParam.ID, ActionParam.Name, ActionParam.Explain, ActionParam.CritExplain, ActionParam.Address, ActionParam.TargetType));
            }
        }

        ChangePhase(Phases.IDLE);
        
        obj = Instantiate(Dummy_Character);
        Dummys[0] = obj;

        obj = Instantiate(Dummy_Character);
        Dummys[1] = obj;

        obj = Instantiate(Dummy_Character);
        Dummys[2] = obj;

        obj = Instantiate(Dummy_Character);
        Dummys[3] = obj;
        Enemies[0] = obj;

        obj = Instantiate(Dummy_Character);
        Dummys[4] = obj;
        Enemies[1] = obj;

        obj = Instantiate(Dummy_Character);
        Dummys[5] = obj;
        Enemies[2] = obj;

        if(StartItem.isContinue == false){
            AppearPlayer(StartItem.StartCharaID[0], 0);
            AppearPlayer(StartItem.StartCharaID[1], 1);
            AppearPlayer(StartItem.StartCharaID[2], 2);
        }else{
            if(ES3.Load<int>("chara1ID") == -1){
                Players[0] = Dummys[0];
            }else{
                AppearPlayer(StartItem.StaticCharaPrefabs[ES3.Load<int>("chara1ID")], 0);
            }
            if(ES3.Load<int>("chara2ID") == -1){
                Players[1] = Dummys[1];
            }else{
                AppearPlayer(StartItem.StaticCharaPrefabs[ES3.Load<int>("chara2ID")], 1);
            }
            if(ES3.Load<int>("chara3ID") == -1){
                Players[2] = Dummys[2];
            }else{
                AppearPlayer(StartItem.StaticCharaPrefabs[ES3.Load<int>("chara3ID")], 2);
            }
        }

    }

    public void AppearPlayer(GameObject Chara, int Number){

        Transform CharaTransform;
        GameObject obj;

        obj = Instantiate(Chara);
        Players[Number] = obj;
        AllObject.Add(obj);
        CharaTransform = Players[Number].transform;
        CharaTransform.Translate (-64.0f-Number*64f, -38.0f, 0.0f);
        Players[Number].GetComponent<Character>().GM = this;
        Players[Number].GetComponent<Character>().GM_I = GM_I;
        Players[Number].GetComponent<Character>().setNumber(Number);
        if(StartItem.isContinue == true){
            Players[Number].GetComponent<Character>().setHP(ES3.Load<int>("chara"+ (Number+1).ToString() +"HP"), Players[Number].GetComponent<Character_Player>().defaultHP);
        }else{
            Players[Number].GetComponent<Character>().setHP(Players[Number].GetComponent<Character_Player>().defaultHP, Players[Number].GetComponent<Character_Player>().defaultHP);
        }
    }

    public void AppearEnemy(List<GameObject> AppearEnemies)
    {
        for(int i=0; i < 3; i++){
            if(AppearEnemies[i] != null){
                GameObject obj = Instantiate(AppearEnemies[i]);
                Enemies[i] = obj;
                AllObject.Add(obj);
                
                Transform CharaTransform = Enemies[i].transform;
                CharaTransform.Translate ((64.0f*i) + 64.0f, -38.0f, 0.0f);
                Vector2 reverse = CharaTransform.localScale;
                reverse.x = -1f;
                CharaTransform.localScale = reverse;

                Enemies[i].GetComponent<Character>().setTeam(true);
                Enemies[i].GetComponent<Character>().GM = this;
            }
        }
        StartCoroutine (BattleLoop());
    }

    private IEnumerator RoundStart()
    {
        Debug.Log("Battle Start!");
        Players[0].GetComponent<Character>().ConsumeTurnEffect();
        Players[1].GetComponent<Character>().ConsumeTurnEffect();
        Players[2].GetComponent<Character>().ConsumeTurnEffect();
        Enemies[0].GetComponent<Character>().ConsumeTurnEffect();
        Enemies[1].GetComponent<Character>().ConsumeTurnEffect();
        Enemies[2].GetComponent<Character>().ConsumeTurnEffect();
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator MainPhase()
    {

        yield return StartCoroutine(EnemyCommand(Enemies[0]));
        yield return StartCoroutine(EnemyCommand(Enemies[1]));
        yield return StartCoroutine(EnemyCommand(Enemies[2]));

        if(Players[0].GetComponent<Character>().isDown == false && Players[0].GetComponent<Character>().getPassive() == GameManager_MainScene.Passives.NanoBot){
            Players[0].GetComponent<Character>().TakeHeal(2);
        }
        if(Players[1].GetComponent<Character>().isDown == false && Players[1].GetComponent<Character>().getPassive() == GameManager_MainScene.Passives.NanoBot){
            Players[1].GetComponent<Character>().TakeHeal(2);
        }
        if(Players[2].GetComponent<Character>().isDown == false && Players[2].GetComponent<Character>().getPassive() == GameManager_MainScene.Passives.NanoBot){
            Players[2].GetComponent<Character>().TakeHeal(2);
        }

        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(PlayerCommand(Players[0]));
        if(Enemies[0].GetComponent<Character>().isDown == true && Enemies[1].GetComponent<Character>().isDown == true && Enemies[2].GetComponent<Character>().isDown == true){
            AllStatusReset();
            yield break;
        }

        yield return StartCoroutine(PlayerCommand(Players[1]));
        if(Enemies[0].GetComponent<Character>().isDown == true && Enemies[1].GetComponent<Character>().isDown == true && Enemies[2].GetComponent<Character>().isDown == true){
            AllStatusReset();
            yield break;
        }

        yield return StartCoroutine(PlayerCommand(Players[2]));
        if(Enemies[0].GetComponent<Character>().isDown == true && Enemies[1].GetComponent<Character>().isDown == true && Enemies[2].GetComponent<Character>().isDown == true){
            AllStatusReset();
            yield break;
        }

        Players[0].GetComponent<Character>().ConsumeTurnEffect(false, false, false, true);    //Reset IsTargeted
        Players[1].GetComponent<Character>().ConsumeTurnEffect(false, false, false, true);
        Players[2].GetComponent<Character>().ConsumeTurnEffect(false, false, false, true);

        yield return StartCoroutine(CharacterAction(Enemies[0]));
        if(Players[0].GetComponent<Character>().isDown == true && Players[1].GetComponent<Character>().isDown == true && Players[2].GetComponent<Character>().isDown == true){
            GameOver();
            yield break;
        }
        yield return StartCoroutine(CharacterAction(Enemies[1]));
        if(Players[0].GetComponent<Character>().isDown == true && Players[1].GetComponent<Character>().isDown == true && Players[2].GetComponent<Character>().isDown == true){
            GameOver();
            yield break;
        }
        yield return StartCoroutine(CharacterAction(Enemies[2]));
        if(Players[0].GetComponent<Character>().isDown == true && Players[1].GetComponent<Character>().isDown == true && Players[2].GetComponent<Character>().isDown == true){
            GameOver();
            yield break;
        }

        yield return StartCoroutine(CharacterAction(Players[0]));
        if(Enemies[0].GetComponent<Character>().isDown == true && Enemies[1].GetComponent<Character>().isDown == true && Enemies[2].GetComponent<Character>().isDown == true){
            AllStatusReset();
            yield break;
        }
        yield return StartCoroutine(CharacterAction(Players[1]));
        if(Enemies[0].GetComponent<Character>().isDown == true && Enemies[1].GetComponent<Character>().isDown == true && Enemies[2].GetComponent<Character>().isDown == true){
            AllStatusReset();
            yield break;
        }
        yield return StartCoroutine(CharacterAction(Players[2]));
        if(Enemies[0].GetComponent<Character>().isDown == true && Enemies[1].GetComponent<Character>().isDown == true && Enemies[2].GetComponent<Character>().isDown == true){
            AllStatusReset();
            yield break;
        }

    }

    private IEnumerator PlayerCommand(GameObject Actor){
        if(Actor.GetComponent<Character>().isDown == false){
            yield return StartCoroutine( CommandSelect_Chara(Actor.GetComponent<Character>()) );
            yield return StartCoroutine( Action(Actor.GetComponent<Character>().getCommand(), Actor, Actor.GetComponent<Character>().getTarget(), true) );
        }
    }

    private IEnumerator EnemyCommand(GameObject Actor){
        if(Actor.GetComponent<Character>().isDown == false){
            List<int> AL = Actor.GetComponent<Character_Enemy_Machine>().getActionList();
            Actor.GetComponent<Character>().setTarget(AISelectTarget(Actor));
            Actor.GetComponent<Character>().setCommand(AL[Random.Range(0, AL.Count())]);
            yield return StartCoroutine( Action(Actor.GetComponent<Character>().getCommand(), Actor, Actor.GetComponent<Character>().getTarget(), true) );
        }
    }

    private IEnumerator CharacterAction(GameObject Actor){
        if(Actor.GetComponent<Character>().isDown == false){
            yield return StartCoroutine( Action(Actor.GetComponent<Character>().getCommand(), Actor, Actor.GetComponent<Character>().getTarget(), false) );
        }
    }

    private IEnumerator CommandSelect_Chara(Character Actor){

        GetComponent<GameManager_TimeScale>().setTimeStop();
        Actor.setCommand(-1);

        GameObject window = Instantiate(CommandWindow);
        window.transform.SetParent(this.transform);
        window.transform.Find("Canvas").GetComponent<Canvas>().worldCamera = window.transform.root.GetComponent<Camera>();
        window.transform.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "Upper UI";

        foreach(SpriteRenderer SR in Actor.transform.Find("SpriteSet").GetComponentsInChildren<SpriteRenderer>() ){
            SR.sortingLayerName = "Unhide Object";
        }

        int MainEquipAddActionA = 0;
        int MainEquipAddActionB = 0;
        int MainEquipAddActionC = 0;
        if(GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == Actor.getNumber()) != null){
            Item MainItem = GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == Actor.getNumber());
            MainEquipAddActionA = MainItem.addActionA;
            MainEquipAddActionB = MainItem.addActionB;
            MainEquipAddActionC = MainItem.addActionC;
        }

        int SubEquipAddActionA = 0;
        int SubEquipAddActionB = 0;
        int SubEquipAddActionC = 0;
        if(GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == Actor.getNumber()+3) != null){
            Item SubItem = GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == Actor.getNumber()+3);
            SubEquipAddActionA = SubItem.addActionA;
            SubEquipAddActionB = SubItem.addActionB;
            SubEquipAddActionC = SubItem.addActionC;
        }

        bool isPistol = false;
        if(GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == Actor.getNumber()) != null){
            if(GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == Actor.getNumber()).getAnimName() == "Pistol")isPistol = true;
        }

        window.GetComponent<CommandWindow>().GM = this;
        window.GetComponent<CommandWindow>().GM_I = this.GM_I;
        window.GetComponent<CommandWindow>().Actor = Actor;
        window.GetComponent<CommandWindow>().setEquipData(
            MainEquipAddActionA,
            MainEquipAddActionB,
            MainEquipAddActionC,
            SubEquipAddActionA,
            SubEquipAddActionB,
            SubEquipAddActionC,
            isPistol
        );

        while(window.GetComponent<CommandWindow>().Command == -1 || ( window.GetComponent<CommandWindow>().TargetType != 0 && window.GetComponent<CommandWindow>().Target == null )){
            yield return null;
        }

        Actor.setCommand(window.GetComponent<CommandWindow>().Command);
        Actor.setIsCrit(window.GetComponent<CommandWindow>().isCrit);
        Actor.setTarget(window.GetComponent<CommandWindow>().Target);
        Destroy(window);

        foreach(SpriteRenderer SR in Actor.transform.Find("SpriteSet").GetComponentsInChildren<SpriteRenderer>() ){
            SR.sortingLayerName = "Default";
        }

        GetComponent<GameManager_TimeScale>().unTimeStop();
    }


    private IEnumerator BattleLoop (){

        bool isHeal = false;
        ChangePhase(Phases.IN_BATTLE);
        yield return StartCoroutine (RoundStart());
        yield return StartCoroutine (MainPhase());
        if( Players[0].GetComponent<Character>().isDown == true && Players[1].GetComponent<Character>().isDown == true && Players[2].GetComponent<Character>().isDown == true ){
            Phase = Phases.OVER;
        }else if( Enemies[0].GetComponent<Character>().isDown == false || Enemies[1].GetComponent<Character>().isDown == false || Enemies[2].GetComponent<Character>().isDown == false ){
            StartCoroutine (BattleLoop());
        }else{

            if(Players[0].GetComponent<Character>().isDown == false && Players[0].GetComponent<Character>().getPassive() == GameManager_MainScene.Passives.NanoBot){
                Players[0].GetComponent<Character>().TakeHeal(99);
                isHeal = true;
            }
            if(Players[1].GetComponent<Character>().isDown == false && Players[1].GetComponent<Character>().getPassive() == GameManager_MainScene.Passives.NanoBot){
                Players[1].GetComponent<Character>().TakeHeal(99);
                isHeal = true;
            }
            if(Players[2].GetComponent<Character>().isDown == false && Players[2].GetComponent<Character>().getPassive() == GameManager_MainScene.Passives.NanoBot){
                Players[2].GetComponent<Character>().TakeHeal(99);
                isHeal = true;
            }
            if(isHeal == true)yield return new WaitForSeconds(1f);
            Debug.Log(GM_E.getLastEvent());
            if(GM_E.getEventList().Count(evs => evs.ID == -(GM_E.getLastEvent())) != 0){
                GM_E.StartEvent(-(GM_E.getLastEvent()));
            }

            ChangePhase(Phases.IDLE);
        }
    }

    public void OpenCharaSlot(){
        if(Players[0].GetComponent<Character>().isDown == true)Players[0] = Dummys[0];
        if(Players[1].GetComponent<Character>().isDown == true)Players[1] = Dummys[1];
        if(Players[2].GetComponent<Character>().isDown == true)Players[2] = Dummys[2];
        if(Enemies[0].GetComponent<Character>().isDown == true)Enemies[0] = Dummys[3];
        if(Enemies[1].GetComponent<Character>().isDown == true)Enemies[1] = Dummys[4];
        if(Enemies[2].GetComponent<Character>().isDown == true)Enemies[2] = Dummys[5];
    }

    public void GameOver(){
        Phase = Phases.OVER;
        GameObject window = Instantiate(GameOverWindow);
        window.transform.SetParent(this.transform);
    }

    public void ChangePhase(Phases Phase){
        this.Phase = Phase;
    }

    public void SwapCharacter(int Num){
        GameObject CharaStock = Players[Num+1];
        Players[Num+1] = Players[Num];
        Players[Num] = CharaStock;
        Players[Num].transform.position += new Vector3(64f, 0, 0);
        Players[Num+1].transform.position += new Vector3(-64f, 0, 0);
        Players[Num].GetComponent<Character>().setNumber( Players[Num].GetComponent<Character>().getNumber()-1 );
        Players[Num+1].GetComponent<Character>().setNumber( Players[Num+1].GetComponent<Character>().getNumber()+1 );

        Item SwapItemA = GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == Num);
        Item SwapItemB = GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == Num+1);
        if(SwapItemA != null)SwapItemA.sortItem(Num+1);
        if(SwapItemB != null)SwapItemB.sortItem(Num);

        SwapItemA = GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == Num+3);
        SwapItemB = GM_I.GetItemDataList().FirstOrDefault(item => item.getSortNo() == Num+4);
        if(SwapItemA != null)SwapItemA.sortItem(Num+4);
        if(SwapItemB != null)SwapItemB.sortItem(Num+3);
    }

    // Update is called once per frame
    void Update()
    {
        if(Phase == Phases.IDLE){

            if(GM_I.GetItemDataList().Count(item => item.getSortNo() >= 100) > 0){
                LootButton.GetComponent<Button>().enabled = true;
                LootButton.GetComponent<Animator>().SetBool("Disabled", false);
            }else{
                LootButton.GetComponent<Button>().enabled = false;
                LootButton.GetComponent<Animator>().SetBool("Disabled", true);
            }

            InventoryButton.GetComponent<Button>().enabled = true;
            InventoryButton.GetComponent<Animator>().SetBool("Disabled", false);
            MapButton.GetComponent<Button>().enabled = true;
            MapButton.GetComponent<Animator>().SetBool("Disabled", false);
        }else{
            LootButton.GetComponent<Button>().enabled = false;
            LootButton.GetComponent<Animator>().SetBool("Disabled", true);
            InventoryButton.GetComponent<Button>().enabled = false;
            InventoryButton.GetComponent<Animator>().SetBool("Disabled", true);
            MapButton.GetComponent<Button>().enabled = false;
            MapButton.GetComponent<Animator>().SetBool("Disabled", true);
        }
    }

}
