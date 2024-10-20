using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

   

    public GameObject pauseMenuUI;

    

    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Restart()
    {
        
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
        
        AudioManager.Instance.UnmuteSound("Theme");

        AudioManager.Instance.SetMasterVolume(AudioSettings.Instance.masterVolume);
        AudioManager.Instance.MuteSound("CaveBoss");
        AudioManager.Instance.MuteSound("BossMusic");

        AudioManager.Instance.PlaySound("Theme");
        
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

}
