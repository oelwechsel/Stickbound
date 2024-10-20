using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Einfügen von Sound in anderen Skripten:
    // FindObjectOfType<AudioManager>().PlaySound("hierDenStringDesAudioClips");


    public Sound[] sounds;
    public Sound[] footstepSounds;

    private static AudioManager instance;


    public ChangeMusicSkript1 changeMusicSkript1;



    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }
            return instance;
        }
    }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach ( Sound s in sounds)
        {
           
            s.source = gameObject.AddComponent<AudioSource>();
            s.volumeSafe = s.volume;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.blend;
            
            
        }

        foreach (Sound s in footstepSounds)
        {

            s.source = gameObject.AddComponent<AudioSource>();
            s.volumeSafe = s.volume;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.blend;

        }


    }

    void Start()
    {
       
           PlaySound("Theme");
       
    }

    private void Update()
    {
        

    }

    public void PlaySound (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.nameOfSoundClip == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " cannot be found!");
            return;
        }
        s.source.volume = s.volumeSafe;
        s.source.Play();
        
    }

    public void PlayFootstepSound(string name)
    {
        Sound s = Array.Find(footstepSounds, sound => sound.nameOfSoundClip == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " cannot be found!");
            return;
        }

        s.source.Play();

    }

    public void MuteSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.nameOfSoundClip == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " cannot be found!");
            return;
        }

        Debug.Log("Muting sound: " + name);
        s.source.volume = 0f;
    }

    public void UnmuteSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.nameOfSoundClip == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " cannot be found!");
            return;
        }

        Debug.Log("Unmuting sound: " + name);
        s.source.volume = s.volumeSafe;
    }

    public void PlayRandomFootStepSound()
    {
        Sound footstepSound = GetRandomFootstepSound();
        if (footstepSound != null)
        {
            PlayFootstepSound(footstepSound.nameOfSoundClip);
        }
    }

    private Sound GetRandomFootstepSound()
    {
        if (footstepSounds.Length == 0)
        {
            Debug.LogWarning("No footstep sounds found!");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, footstepSounds.Length);
        return footstepSounds[randomIndex];
    }

    public void SetMasterVolume(float volume)
    {
        foreach (Sound s in sounds)
        {
            if (s.source.volume > 0f) // Überprüfe, ob der Sound nicht gemutet ist
            {
                s.source.volume = s.volumeSafe * volume;
            }
        }

        foreach (Sound s in footstepSounds)
        {
            if (s.source.volume > 0f) // Überprüfe, ob der Sound nicht gemutet ist
            {
                s.source.volume = s.volumeSafe * volume;
            }
        }
    }
}
