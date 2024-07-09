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
    public const string Cards = "Cards";
    //public const string AlarmTime = "AlarmTime";
}

public enum MusicZones
{
    Dream = 0,
    Settlement = 1,
    SquareTown = 2,
    GasStation = 3,
    Credits = 4,

    //Campfire,
    //Prueba

}

public enum EventFolders
{
    Combat = 0,
    SFX = 1,
    Music = 2,
    
}

public static class CombatSounds
{
    public const string Flojo = "Flojo";
    public const string Medio = "Medio";
    public const string Fuerte = "Fuerte";
}

public enum CardSounds
{
    Center = 0,
    Left = 1,
    Right = 2,
    Phone = 3

}

//Este enum se refiere al evento de fmod que se utilizara
public enum SoundEvent
{
    StoryEvent1,
    CombatEvent
}