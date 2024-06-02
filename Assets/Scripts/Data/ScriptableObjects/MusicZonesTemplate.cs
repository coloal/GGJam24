using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MusicZonesData", menuName = "MusicZonesData")]
public class MusicZonesTemplate : ScriptableObject
{
    public List<ZoneSoundValues> Zones;

}

[System.Serializable]
public class ZoneSoundValues
{
    public SoundEvent FMODEvent;
    public MusicZones zone;
    
    //public List<SoundAction> soundActions;

    public float Acordeonista;
    public float Bajista;
    public float Cavaquinhista;
    public float Flautista;
    public float Percusionista;
    public float Epic;
    public float Fight;
    public float Zapato;
    public float VolumenHoguera;
    public float ApagarHoguera;








}
