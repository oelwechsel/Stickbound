using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterBossMusic : MonoBehaviour
{
    public EndBossIsDead dead;
    private bool hasPlayedMusic = false; // Neue Variable hinzugefügt

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayedMusic) // Nur wenn der Sound noch nicht abgespielt wurde
        {
            AudioManager.Instance.MuteSound("BossMusic");
            AudioManager.Instance.UnmuteSound("CaveBoss");
            AudioManager.Instance.PlaySound("CaveBoss");
            hasPlayedMusic = true; // Setze die Variable auf true, um zukünftige Wiedergaben zu verhindern
        }
    }

    // Du könntest dies hier weglassen, da das Ereignis im Trigger erfolgt

    /*private void Update()
    {
        if (dead != null)
        {
            if (dead.bossIsDead)
            {
                AudioManager.Instance.MuteSound("BossMusic");
                AudioManager.Instance.UnmuteSound("CaveBoss");
                AudioManager.Instance.PlaySound("CaveBoss");
            }
        }
    }*/
}
