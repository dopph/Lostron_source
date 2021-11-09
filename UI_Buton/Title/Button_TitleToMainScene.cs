using UnityEngine;
using UnityEngine.SceneManagement;
 
public class Button_TitleToMainScene : MonoBehaviour {
 
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("StartItemScene");
    }
 
}