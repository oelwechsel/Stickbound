using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWallSkript : MonoBehaviour
{
     public EndBossIsDead endBossisDeadSkript;

    public GameObject bossWall;


    private void Awake()
    {
        //bossWall = GameObject.Find("BossWallObject").GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(endBossisDeadSkript != null)
        {
            if(bossWall != null)
            {
                if (endBossisDeadSkript.bossIsDead)
                {
                    bossWall.SetActive(false);
                }
                else
                {
                    bossWall.SetActive(true);
                }
            }
        }

    }
}
