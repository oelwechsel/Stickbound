using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCheckPoint : MonoBehaviour
{

    PlayerStats playerStats;
    public bool isInEndCheckPoint;

    private void Awake()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInEndCheckPoint = true;
            playerStats.currentHealth = playerStats.maxHealth;
            
        }
        else
        {
            isInEndCheckPoint = false;
        }
    }
}
