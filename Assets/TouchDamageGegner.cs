using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDamageGegner : MonoBehaviour
{

    public PlayerStats playerStats;
    public MovementPlayer playerMove;



    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("damage");
                playerStats.DecreaseHealth(10f);
            }
        }
    }


   
}
