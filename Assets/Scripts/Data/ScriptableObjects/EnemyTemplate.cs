using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTemplate", menuName = "Enemy Template")]
public class EnemyTemplate : ScriptableObject
{
    public Sprite characterSprite;
    public List<CombatCardTemplate> CombatCards;
    public List<string> OnStartConversation;
    public List<string> OnWinConversation;
    public List<string> OnLoseConversation;
    public float ShowHintsTimeInSeconds;

    
}
