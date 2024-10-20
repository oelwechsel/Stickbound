using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;


    [SerializeField]
    private MovementPlayer PlayerMove;

    private Rigidbody2D rb;

    [SerializeField]
    private float damageSpikes = 10f;
    [SerializeField]
    private float knockback = 2f;

    //private bool canDamage = true;
    //private float damageCooldown = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        
            if (collision.gameObject.name == "Player" || collision.gameObject.name == "Player(Clone)")
            {
                //canDamage = false;
                //if (canDamage)
                playerStats.DecreaseHealth(damageSpikes);
               FindObjectOfType<AudioManager>().PlaySound("PlayerHit");
            PlayerMove.KnockbackSpikes(knockback);

            }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //canDamage = false;
    }



}
