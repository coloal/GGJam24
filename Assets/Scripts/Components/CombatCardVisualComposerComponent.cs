using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatCardVisualComposerComponent : MonoBehaviour
{
   
    public Sprite GetCardCharacterSprite(CombatCardTemplate combatCardTemplate)
    {
        return combatCardTemplate.cardSprite;
    }
}
