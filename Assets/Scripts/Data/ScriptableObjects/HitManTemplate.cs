using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ScenesNames;

[CreateAssetMenu(fileName = "New HitManData", menuName = "HitMan")]
public class HitManTemplate : ScriptableObject
{
    //Type of HitMan
    public HitManTypes HitManType;
    
    //Information of the card
    public string NameOfHitman;
    public string Description;
    
    //Stats
    public int ViolenceStat; 
    public int MoneyStat;
    public int InfluenceStat;

    //HitMan sprite
    public Sprite HitManSprite;

}
