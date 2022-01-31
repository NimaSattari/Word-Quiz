using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Setting;
    int LevelPassed;

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadLevelMenu()
    {
        SceneManager.LoadScene("LevelSelection");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void SettingShowAndHide()
    {
        if(Setting.active == true)
        {
            Setting.SetActive(false);
        }
        else
        {
            Setting.SetActive(true);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Play()
    {
        LevelPassed = PlayerPrefs.GetInt("LevelPassed", LevelPassed);
        if (LevelPassed == 0)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(LevelPassed + 1);
        }
    }
    public void LoadThisLevel(int level)
    {
        SceneManager.LoadScene(level + 1);
    }
}
