using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField] private string StoryEventPath = "event:/MaquetaAudioLeve";
    private FMOD.Studio.EventInstance FModInstance;

    // Singleton instance
    public static AudioManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        FModInstance = FMODUnity.RuntimeManager.CreateInstance(StoryEventPath);
    }

    public void Play()
    {
        FModInstance.start();

        FModInstance.setParameterByName("Flautista", 1f);
        float f;
        FModInstance.getParameterByName("Flautista",out f);
        f -= 0.25f;

        FModInstance.setParameterByName("Flautista", f);
    }

    /*
    Musica:
    Por zonas:
    -cambia el evento
    -se autoajustan parametros

    implementaci�n:
    scriptableObject:
    Zona(Enum) y valores que debe tomar cada var del evento de historia

    Por Acciones:
    Las cartas contendran actions si se tira izq o derecha estas Actions activaran/desactivaran los eventos
    
    Accion NumIntegrantes:
    Acci�n que autoajusta como van a sonar los instrumentos de fondo por multiplicadores ??(preguntar a Omar)
    Esta acci�n se llamara al finalizar un combate/cuando se reclute alguien para actualizar el balance de tipos

     */



    //-----------------------------------------------------------------
    /*
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
    }*/
}
