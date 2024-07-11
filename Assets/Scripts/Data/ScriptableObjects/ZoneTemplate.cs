using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ZoneData", menuName = "ZoneData")]
public class ZoneTemplate : ScriptableObject
{
    //Story information
    [Header("Story information")]
    [SerializeField]
    Sprite storyBackgroundSprite;
    [SerializeField]
    MusicZones storyMusicZone;
    [SerializeField]
    Animator zoneTransition;
    [SerializeField]
    Material zoneMaterial;
    //Combat Information
    [Header("Combat information")]
    [SerializeField]
    Animator combatTransition;

    public Sprite StoryBackgroundSprite => storyBackgroundSprite;
    public MusicZones StoryMusicZone => storyMusicZone;
    public Animator ZoneTransition => zoneTransition;
    public Animator CombatTransition => combatTransition;
    public Material ZoneMaterial => zoneMaterial;
}
