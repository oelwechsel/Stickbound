using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicSkript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.MuteSound("CaveBoss");
        AudioManager.Instance.UnmuteSound("BossMusic");
        AudioManager.Instance.PlaySound("BossMusic");
    }
}
