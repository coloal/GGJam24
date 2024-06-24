using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTemplate", menuName = "Enemy Template")]
public class EnemyTemplate : ScriptableObject
{
    public List<CombatCardTemplate> CombatCards;
    public float ShowHintsTimeInSeconds;

    
}
