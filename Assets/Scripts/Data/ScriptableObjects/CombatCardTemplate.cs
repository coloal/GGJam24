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

[CreateAssetMenu(fileName = "New CombatCardData", menuName = "CombatCardData")]
public class CombatCardTemplate : ScriptableObject
{
    public string NameOfCard;

    //Dialog Text
    public string InitialText;
    public string EffectiveText;
    public string NonEffectiveText;

    //Card sprite
    public Sprite CardSprite;
    public Sprite BackgroundSprite;

    //Combat card stats
    public int HealthPoints = 0;
    public int Damage = 0;
    public int Armor = 0;
    public int Turns = 0;
    public CombatTypes CombatType;

    //Instrument associated
    public string Instrument;
}