using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    private static AudioSettings instance;
    public static AudioSettings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioSettings>();
            }
            return instance;
        }
    }

    public float masterVolume = 1f;

    private void Awake()
    {
        
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject); // Stelle sicher, dass das GameObject zwischen Szenen erhalten bleibt
            }
            else
            {
                Destroy(gameObject); // Falls ein weiteres GameObject erstellt wird, zerstöre es
            }
        
    }
}
