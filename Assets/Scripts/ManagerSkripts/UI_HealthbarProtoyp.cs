using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HealthbarProtoyp : MonoBehaviour
{
    public PlayerStats playerStats;
    public Image fillImage;
    public TextMeshProUGUI healthText; // TextMesh Pro-Komponente für die Gesundheitsanzeige

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }

        if (slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }

        float fillValue = playerStats.currentHealth / playerStats.maxHealth;
        slider.value = fillValue;

        // Aktualisiere den Gesundheitswert im Text im Format "maxHealth/currentHealth"
        healthText.text = $"{playerStats.currentHealth}/{playerStats.maxHealth}";
    }
}

