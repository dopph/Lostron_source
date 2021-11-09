using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartItem : MonoBehaviour
{
    public static int StartItemID = 0;
    public static GameObject[] StartCharaID = new GameObject[3];
    
    public static float BGM_Volume = 0.5f;
    public static float SE_Volume = 0.5f;
    public static bool isContinue = false;
    public static GameObject[] StaticCharaPrefabs;
    public GameObject[] CharaPrefabs;

    [SerializeField] ActionsDB ActionDBsheet;
    private List<ActionCard> ActionCardDB = new List<ActionCard>();
    public List<ActionCard> actionCardDB{
        get{return ActionCardDB;} set{ActionCardDB = value;}
    }
    void Start(){

        foreach (ActionsDB.Sheet ActionSheet in ActionDBsheet.sheets) {
            //各項目のデータを順に取り出す
            foreach (ActionsDB.Param ActionParam in ActionSheet.list) { 
                ActionCardDB.Add(new ActionCard(ActionParam.ID, ActionParam.Name, ActionParam.Explain, ActionParam.CritExplain, ActionParam.Address, ActionParam.TargetType));
            }
        }
        
        StaticCharaPrefabs = CharaPrefabs;
        StartItem.BGM_Volume = ES3.Load<int>("BGM_Volume", defaultValue:50);
        StartItem.SE_Volume = ES3.Load<int>("SE_Volume", defaultValue:50);
        
    }

}
