using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenAppear : MonoBehaviour
{
    public string sceneToLoad; // Der Name der Szene, die du laden möchtest

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Stelle sicher, dass der Collider vom Spieler getroffen wird
        {
            SceneManager.LoadScene(sceneToLoad); // Lade die angegebene Szene
        }
    }
}
