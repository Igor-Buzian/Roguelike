using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLogic : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("GenerationScene");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void RespawnButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GenerationScene");
    }
    public void BackToMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
}
