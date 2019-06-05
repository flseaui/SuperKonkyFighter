using UnityEngine;

namespace Core
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource EffectsSource;
        public AudioSource MusicSource;
    
        public static AudioManager Instance;

        public AudioClip[] MusicClips;
        public AudioClip[] SoundClips;

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
            MenuTheme,
            VsTheme,
            KonkysTheme,
            GreyshirtsTheme,
            ShrulksTheme,
            ShrulksAlt,
            Training,
            ShrulksAltAltRight
        } 

        public enum Sound
        {
            Light,
            Medium,
            Heavy,
            Block,
            Fireball,
            Whiff,
            Super,
            PushBlock,
            GunShot,
            Dp,
        }

        public void PlaySound(Sound? sound)
        {
            if (sound is null) return;
            EffectsSource.clip = SoundClips[(int) sound];
            EffectsSource.Play();
        }

        public void PlayMusic(Music? sound)
        {
            if (sound is null) return;
            MusicSource.clip = MusicClips[(int) sound];
            MusicSource.Play();
        }

    }
}