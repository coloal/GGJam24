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

    //Combat Information
    [Header("Combat information")]
    [SerializeField]
    Sprite combatBackgroundSprite;
    [SerializeField]
    List<Sprite> CombatTurnSprites;
}
