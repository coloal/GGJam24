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


[CreateAssetMenu(fileName = "New StoryCardData", menuName = "StoryCardData")]
public class StoryCardTemplate : ScriptableObject
{
    //Information of the card
    public string NameOfCard;
    public string Text;

    //Choices text
    public string LeftText;
    public string RightText;

    //Card sprite
    public Sprite CardSprite;
}
