using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BrainSoundTag
{
    public const string Zone = "Zone";
    public const string Acordeon = "Acordeon";
    public const string Clavicordio = "Clavicordio";
    public const string Guitar = "Guitar";
    public const string Ness = "Ness";

    public const string LetsFight = "LetsFight";
    public const string FinBatalla = "FinBatalla";
    
    public const string Curse = "Curse";
    
    public const string ApagarHoguera = "ApagarHogera";
    public const string Golpes = "Golpes";
}

public enum MusicZones
{
    Settlement = 1,
    SquareTown = 2,
    GasStation = 3,
    //Campfire,
    //Prueba

}

public static class CombatSounds
{
    public const string Flojo = "Flojo";
    public const string Medio = "Medio";
    public const string Fuerte = "Fuerte";
}

//Este enum se refiere al evento de fmod que se utilizara
public enum SoundEvent
{
    StoryEvent1,
    CombatEvent
}