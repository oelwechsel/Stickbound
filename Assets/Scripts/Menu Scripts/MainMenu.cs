using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   // public Slider audioSlider;

    private void Start()
    {
       // audioSlider.value = AudioSettings.Instance.masterVolume;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    //public void OnAudioSliderValueChanged(float value)
    //{
    //    AudioSettings.Instance.masterVolume = value;
    //    AudioManager.Instance.SetMasterVolume(value);
    //}

}
