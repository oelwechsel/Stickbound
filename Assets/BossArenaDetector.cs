using UnityEngine;

public class BossArenaDetector : MonoBehaviour
{
    public BossHealthbarUI bossHealthbarUI; // Verweise hier auf dein UI-Skript
    public EndBossIsDead dead;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            if(!dead.bossIsDead)
            {
                bossHealthbarUI.ShowHealthBar(true);
            }
            else
            {
                bossHealthbarUI.ShowHealthBar(false);
            }
           
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossHealthbarUI.ShowHealthBar(false);
        }
    }
}
