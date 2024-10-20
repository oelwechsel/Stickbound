using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbarUI : MonoBehaviour
{
    public Slider healthSlider;
    public Image fillImage; // Referenz auf das FillImage des Sliders
    public TextMeshProUGUI healthText;

    public EndBossIsDead endBossDead;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // Aktualisiere die Healthbar basierend auf den Healthwerten
        float healthPercentage = currentHealth / maxHealth;
        healthSlider.value = healthPercentage;

        // Zeige den Health-Text im Format "currentHealth/maxHealth" an
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    public void ShowHealthBar(bool show)
    {
        
            //healthText.gameObject.SetActive(show);
            healthSlider.gameObject.SetActive(show);
        //AudioManager.Instance.MuteSound("CaveBoss");
        //AudioManager.Instance.PlaySound("PlayerDash");
        //AudioManager.Instance.PlaySound("BossMusic");
        // Zeige oder blende die Healthbar basierend auf "show" ein oder aus
       
    }

    private void Update()
    {

        if (endBossDead.bossIsDead)
        {
            ShowHealthBar(false);
        }


        //Wenn low on Health
        if (healthSlider.value <= healthSlider.minValue)
        {
            healthText.enabled = false;
            fillImage.enabled = false;
        }

        if (healthSlider.value > healthSlider.minValue && !fillImage.enabled)
        {
            healthText.enabled = true;
            fillImage.enabled = true;
        }
    }
}
