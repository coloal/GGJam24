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
    MusicZonesTemplate storyMusicZone;
    [SerializeField]
    Animator zoneTransition;

    //Combat Information
    [Header("Combat information")]
    [SerializeField]
    Sprite combatBackgroundSprite;
    [SerializeField]
    List<Sprite> combatTurnSprites;
    [SerializeField]
    Animator combatTransition;

    public Sprite StoryBackgroundSprite => storyBackgroundSprite;
    public MusicZonesTemplate StoryMusicZone => storyMusicZone;
    public Sprite CombatBackgroundSprite => combatBackgroundSprite;
    public List<Sprite> CombatTurnSprites => combatTurnSprites;
    public Animator ZoneTransition => zoneTransition;
    public Animator CombatTransition => combatTransition;
}
