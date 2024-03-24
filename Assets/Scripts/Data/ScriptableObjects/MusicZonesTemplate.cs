using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


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
    public float Acordeonista;
    public float Bajista;
    public float Cavaquinhista;
    public float Epic;
    public float Fight;
    public float Flautista;
    public float Percusionista;
    public float Zapato;

}
