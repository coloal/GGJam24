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

    //Combat Information
    [Header("Combat information")]
    [SerializeField]
    Sprite combatBackgroundSprite;
    [SerializeField]
    Sprite combatTurnsContainerSprite;
    [SerializeField]
    List<Sprite> combatTurnSprites;
    [SerializeField]
    Animator combatTransition;

    public Sprite StoryBackgroundSprite => storyBackgroundSprite;
    public MusicZones StoryMusicZone => storyMusicZone;
    public Sprite CombatBackgroundSprite => combatBackgroundSprite;
    public Sprite CombatTurnsContainerSprite => combatTurnsContainerSprite;
    public List<Sprite> CombatTurnSprites => combatTurnSprites;
    public Animator ZoneTransition => zoneTransition;
    public Animator CombatTransition => combatTransition;
}
