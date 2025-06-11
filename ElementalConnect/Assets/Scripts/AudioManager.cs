using System;
using UnityEngine;

/// <summary>
/// Manages the audio functionality of the game, including music and sound effects.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    /// <summary>
    /// Initializes the AudioManager as a singleton and ensures it persists across scene loads.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad((gameObject));
        }
    }

    /// <summary>
    /// Plays the default menu music at the start of the game.
    /// </summary>
    public void Start()
    {
        PlayMusic("Menu");
    }

    /// <summary>
    /// Plays the specified music track by searching for its name in the musicSounds array.
    /// </summary>
    /// <param name="name">The name of the music track to be played.</param>
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x=>x.name == name);
        
        if (s != null)
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    /// <summary>
    /// Plays the specified sound effect by searching for its name in the sfxSounds array.
    /// </summary>
    /// <param name="name">The name of the sound effect to be played.</param>
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s != null)
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    /// <summary>
    /// Toggles the mute state of the music source.
    /// </summary>
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    /// <summary>
    /// Toggles the mute state of the sound effects source.
    /// </summary>
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
}
