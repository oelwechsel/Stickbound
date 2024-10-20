using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
//using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public float maxHealth;

    public float currentHealth;

    public Light2D light1;
    public Light2D light2;

    private Color originalLight1Color;
    private Color originalLight2Color;

    private float originalLight1Intensity;
    private float originalLight2Intensity;


    SpriteRenderer spriteRenderer;
    private GameManager gameManager;


    public float invulnerabilityDuration = 2.0f; // Dauer der Unverwundbarkeit in Sekunden
    public bool isInvulnerable = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        originalLight1Color = light1.color;
        originalLight2Color = light2.color;

        originalLight1Intensity = light1.intensity;
        originalLight2Intensity = light2.intensity;
    }

    private void Update()
    {
        ClampHealth();
       // Debug.Log(currentHealth);

        if(isInvulnerable)
        {
            //Color currentColor = spriteRenderer.color;
            //currentColor.a = 0.5f;
            //spriteRenderer.color = currentColor;


            light1.color = Color.red;
            light2.color = Color.red;

            light1.intensity = originalLight1Intensity * 0.5f; // Verringere die Intensität um die Hälfte
            light2.intensity = originalLight2Intensity * 0.5f;


            //spriteRenderer.color = Color.green;

        }
        else
        {
            //Color currentColor = spriteRenderer.color;
            //currentColor.a = 1f;
            //spriteRenderer.color = currentColor;
            //spriteRenderer.color = Color.white;
            light1.color = originalLight1Color;
            light2.color = originalLight2Color;

            light1.intensity = originalLight1Intensity;
            light2.intensity = originalLight2Intensity;
        }

    }


    private void ClampHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }




    public void DecreaseHealth(float amount)
    {

        if (!isInvulnerable)
        {
            
            currentHealth -= amount;
            FindObjectOfType<AudioManager>().UnmuteSound("PlayerHit");
            FindObjectOfType<AudioManager>().PlaySound("PlayerHit");
            Debug.Log("Current Health: " + currentHealth);

            if (currentHealth <= 0.0f)
            {
                currentHealth = 0f;
                Die();
                
            }
            else
            {
                StartInvulnerability();
            }
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    private void Die()
    {
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        //SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Single);
        //transform.position = new Vector3(1, 1, 1);
        gameManager.Respawn();
        //gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    private void StartInvulnerability()
    {
        
        isInvulnerable = true;
        //FindObjectOfType<AudioManager>().MuteSound("PlayerHit");

        Invoke("EndInvulnerability", invulnerabilityDuration);
    }

    private void EndInvulnerability()
    {
        isInvulnerable = false;
    }

    public float ShowCurrentHealth()
    {
       
        return currentHealth;
    }

}
