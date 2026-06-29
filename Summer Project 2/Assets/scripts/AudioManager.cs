using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    // singleton variable, meaning theres only one instance preventing copies being made
    public static AudioManager Instance {get; private set;}

    [SerializeField, Tooltip("used to store all the sounds in the game")]
    private Sound[] sounds;

    private void Awake()
    {
        // if there is no instance of the audio manager assigned in the game
        if (Instance == null)
        {
            // assign this instance of this script as the "Instance" 
            Instance = this;
        }
        else
        {
            // Destroy the duplicates
            Destroy(this);
        }
        // stops the object from being destroyed when reloading the scene
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            // Add an audio source component to this manager for the given object
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
    }

    private void Start()
    {
        PlaySound("Theme");
    }
    public void PlaySound(string name)
    {
        // Find the sound in the sounds array based on the name passed in 
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        // Check if we found the sound
        if (sound == null)
        {
            Debug.LogWarning($"Could not find {name} sound!");
            return; // Stop the function
        }

        Debug.Log($"Playing {name} sound");
        // Play the sound
        sound.audioSource.Play();
    }

    public void StopSound(string name)
    {
        // Find the sound in the sounds array based on the name passed in 
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        // Check if we found the sound
        if (sound == null)
        {
            Debug.LogWarning($"Could not find {name} sound!");
            return; // Stop the function
        }
        // Play the sound
        sound.audioSource.Stop();
    }
}
