using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_DecideItem : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("MainScene");
    }
}
