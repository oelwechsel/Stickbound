using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{

    public MovementPlayer player;
    public PlayerStats playerStats;
    public PlayerCombatTry playerCombat;



    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<MovementPlayer>();
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        playerCombat = GameObject.Find("Player").GetComponent<PlayerCombatTry>();
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //playerStats.DecreaseHealth(10);
            Debug.Log("Touiched");
            player.KnockbackSpikes(10);
        }
    }

}
