using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    private CheckPointManager checkpointManager;
    private bool isReached = false;

    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    public Light2D lightCheckpoint;

    private PlayerStats playerStats;

    public GameObject particlesCheckpoint;




    private void Start()
    {
        
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        lightCheckpoint.enabled = false;
        particlesCheckpoint.SetActive(false);

        spriteRenderer =GetComponent<SpriteRenderer>();
        checkpointManager = FindObjectOfType<CheckPointManager>();
        checkpointManager.RegisterCheckpoint(this);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isReached)
        {
            //restore Health on Checkpoint
            if(playerStats.currentHealth <= playerStats.maxHealth - 20f)
            {
                playerStats.currentHealth = playerStats.currentHealth + 20f;
            }
            else if(playerStats.currentHealth == playerStats.maxHealth - 10f) 
            {
                playerStats.currentHealth = playerStats.currentHealth + 10f;
            }

            particlesCheckpoint.SetActive(true);
            lightCheckpoint.enabled = true;
            spriteRenderer.sprite = sprites[0];
            spriteRenderer.color = Color.yellow;
            FindObjectOfType<AudioManager>().PlaySound("CheckPointAberEigDash");
            checkpointManager.SetLastCheckpoint(this);
            isReached = true;
            // Weitere Aktionen, z.B. Aktivieren des Checkpoint-Sprites
        }
    }
}
