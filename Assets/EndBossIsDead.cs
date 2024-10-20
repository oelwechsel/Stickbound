using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBossIsDead : MonoBehaviour
{

    private GameObject aliveGO;
    public bool bossIsDead;

        private void Awake()
    {
        aliveGO = transform.Find("Alive").gameObject;
    }


    private void Update()
    {
        if (aliveGO != null)
        {
            if (!aliveGO.activeSelf)
            {
                bossIsDead = true;
            }
            else
            {
                bossIsDead = false;
            }
        }
        //Debug.Log(bossIsDead);
    }
}

    
