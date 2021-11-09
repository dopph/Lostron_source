using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

public class Button_MainSceneToTitle : MonoBehaviour {
 
    private GameObject window;
    private GameManager_TimeScale GM_T;

    void Start(){
        GM_T = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_TimeScale>();
    }

    public void OnClickStartButton(){
        Addressables.LoadAsset<GameObject>("Assets/UI/Prefab/CommonDialog.prefab").Completed += op => { 
            window = Instantiate(op.Result);
            window.transform.SetParent(transform.root);
            window.GetComponent<CommonDialog>().SetTitleText("タイトルに戻る");
            window.GetComponent<CommonDialog>().SetMainText("本当にタイトルに戻りますか？");
        };
    }
 
    void Update(){
        if(window != null){
            if(window.GetComponent<CommonDialog>().GetValue() == 1){
                GM_T.unTimeStop(true);
                SceneManager.LoadScene("Title");
            }
        }else{
        }

    }

     /*
    [SerializeField] GameObject checkWindow;
    private GameManager_TimeScale GM_T;

    void Start(){
        GM_T = transform.root.transform.Find("GameManager").gameObject.GetComponent<GameManager_TimeScale>();
    }

    public void OnClickStartButton(){
        StartCoroutine(CheckExitGame());
    }
 
    private IEnumerator CheckExitGame(){
        
        GameObject window = Instantiate(checkWindow);
        window.GetComponent<CommonDialog>().SetTitleText("タイトルに戻る");
        window.GetComponent<CommonDialog>().SetMainText("本当にタイトルに戻りますか？");
        window.transform.SetParent(transform.root);

        while(checkWindow.GetComponent<CommonDialog>().GetValue() == -1){
            yield return null;
        }
        Debug.Log(checkWindow.GetComponent<CommonDialog>().GetValue());
        if(checkWindow.GetComponent<CommonDialog>().GetValue() == 1){
            GM_T.unTimeStop();
            SceneManager.LoadScene("Title");
        }

    }
    */

}