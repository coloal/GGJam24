using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatVisualManager : MonoBehaviour
{
    [Header("Scene visual configurations")]
    [SerializeField] private SpriteRenderer sceneBackgroundSpriteRenderer;

    [Header("Debug")]
    [SerializeField] private List<Sprite> debugTurnsSprites;

    //private ???? zoneAssets
    private Dictionary<string, Sprite> numberSpritesDictionary;

    void Start()
    {
        //TODO: Get from GameManager the current zone assets
        
        // Debug
        InitNumberSpritesDictionary(debugTurnsSprites);
    }

    void InitNumberSpritesDictionary(List<Sprite> numberImages)
    {
        numberSpritesDictionary = new Dictionary<string, Sprite>();
        for (int i = 0; i < numberImages.Count; i++)
        {
            numberSpritesDictionary.Add(i.ToString(), numberImages[i]);
        }
    }

    public (Sprite, Sprite) GetTurnNumberAsSprites(string turnAsString)
    {
        Sprite unitNumberSprite = null;
        Sprite tensNumberSprite = null;

        if (turnAsString.Length == 1)
        {
            unitNumberSprite = numberSpritesDictionary[turnAsString[0].ToString()];
        }
        else if (turnAsString.Length > 1)
        {
            unitNumberSprite = numberSpritesDictionary[turnAsString[0].ToString()];
            tensNumberSprite = numberSpritesDictionary[turnAsString[1].ToString()];
        }

        return (tensNumberSprite, unitNumberSprite);
    }
}
