using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    // Singleton instance
    public static AudioManager Instance;

    public Sound[] Sounds;

    void Awake() {
        void SetUpSounds() {
            foreach (Sound sound in Sounds) {
                sound.AudioSource = gameObject.AddComponent<AudioSource>();
                sound.AudioSource.clip = sound.AudioClip;
                sound.AudioSource.volume = sound.Volume;
                sound.AudioSource.pitch = sound.Pitch;
                sound.AudioSource.loop = sound.Loop;
                sound.IsPlaying = false;
            }
        }

        // Singleton configuration
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        SetUpSounds();
    }

    public void Play(string name) {
        Sound sound = Array.Find(Sounds, s => s.Name == name);
        if (sound != null) {
            // Stop any background music that is playing if the sound being played is another BGM
            if (sound.IsBGM) {
                Sound playingBGM = Array.Find(Sounds, s => s.IsBGM && s.IsPlaying);
                if (playingBGM != null) {
                    Stop(playingBGM.Name);
                }
            }

            sound.AudioSource.Play();
            sound.IsPlaying = true;
        }
    }

    public void PlayBgmWithoutInterruption(string name) {
        Sound sound = Array.Find(Sounds, s => s.Name == name);
        if (sound != null) {
            // Stop any background music that is playing if the sound being played is another BGM
            if (sound.IsBGM) {
                Sound playingBGM = Array.Find(Sounds, s => s.IsBGM && s.IsPlaying);
                if (playingBGM == null) {
                    sound.AudioSource.Play();
                    sound.IsPlaying = true;
                } else if (playingBGM.Name != name) {
                    Stop(playingBGM.Name);
                    sound.AudioSource.Play();
                    sound.IsPlaying = true;
                }
            }
        }
    }

    public void Stop(string name) {
        Sound sound = Array.Find(Sounds, s => s.Name == name);
        if (sound != null) {
            sound.AudioSource.Stop();
            sound.IsPlaying = false;
        }
    }
}
