using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound {

    public string Name;

    public AudioClip AudioClip;

    [Range(0f, 1f)]
    public float Volume;

    [Range(.1f, 3f)]
    public float Pitch;

    public bool Loop;

    [HideInInspector]
    public AudioSource AudioSource;

    [HideInInspector]
    public bool IsPlaying;

    public bool IsBGM;
}