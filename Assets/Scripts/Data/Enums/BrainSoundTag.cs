using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BrainSoundTag
{
    public const string Acordeonista = "Acordeonista";
    public const string Bajista = "Bajista";
    public const string Cavaquinhista = "Cavaquinhista";
    public const string Flautista = "Flautista";
    public const string Percusionista = "Percusionista";
    public const string ApagarHoguera = "ApagarHogera";
    public const string VolumenHoguera = "VolumenHoguera";
    public const string Epic = "Epic";
    public const string Fight = "Fight";
    public const string Zapato = "Zapato";

}

public enum MusicZones
{
    AdmiredTown,
    Campfire,
    Prueba

}

//Este enum se refiere al evento de fmod que se utilizara
public enum SoundEvent
{
    StoryEvent1,
    CombatEvent
}