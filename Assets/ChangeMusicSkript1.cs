using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicSkript1 : MonoBehaviour
{
    public bool isInTunnel;


    private void OnTriggerExit2D(Collider2D collision)
    {
        isInTunnel = true;
        FindObjectOfType<AudioManager>().UnmuteSound("CaveBoss");
        FindObjectOfType<AudioManager>().MuteSound("Theme");
        FindObjectOfType<AudioManager>().PlaySound("CaveBoss");
    }
}
