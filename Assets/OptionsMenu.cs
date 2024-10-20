using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider audioSlider;

    private void Start()
    {
        audioSlider.value = AudioSettings.Instance.masterVolume;
    }

    public void OnAudioSliderValueChanged(float value)
    {
        AudioSettings.Instance.masterVolume = value;
        AudioManager.Instance.SetMasterVolume(value);
    }
}
