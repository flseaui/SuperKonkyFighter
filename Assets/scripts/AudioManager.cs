using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource EffectsSource;
    public AudioSource MusicSource;
    
    public static AudioManager Instance = null;

    public AudioClip[] musicClips;
    public AudioClip[] soundClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public enum Music
    {
        MENU_THEME,
        VS_THEME,
        KONKYS_THEME,
        GREYSHIRTS_THEME,
        SHRULKS_THEME,
        SHRULKS_ALT,
        TRAINING,
        SHRULKS_ALT_ALT_RIGHT
    } 

    public enum Sound
    {
        LIGHT,
        MEDIUM,
        HEAVY,
        BLOCK,
        FIREBALL,
        KONKY_SUPER,
        WHIFF,
        SUPER,
        PUSHBLOCK,
        GUN_SHOT,
    }

    public void PlaySound(Sound? sound)
    {
        EffectsSource.clip = soundClips[(int)sound];
        EffectsSource.Play();
    }

    public void PlayMusic(Music? sound)
    {
        MusicSource.clip = musicClips[(int)sound];
        MusicSource.Play();
    }

}