using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SpritesUtils
{
    public static (Sprite, Sprite) GetNumbersAsSprites(this Dictionary<string, Sprite> spritesDictionary, int number)
    {
        Sprite unitNumberSprite = null;
        Sprite tensNumberSprite = null;

        string numberAsString = number.ToString();

        if (numberAsString.Length == 1)
        {
            unitNumberSprite = spritesDictionary[numberAsString[0].ToString()];
        }
        else if (numberAsString.Length > 1)
        {
            unitNumberSprite = spritesDictionary[numberAsString[0].ToString()];
            tensNumberSprite = spritesDictionary[numberAsString[1].ToString()];
        }

        return (tensNumberSprite, unitNumberSprite);
    }

    public static void SetNumberAsSprites(GameObject unitNumberContainer, GameObject tensNumberContainer,
        Image unitNumberImage, Image tensUnitNumberImage, Image tensTensNumberImage,
        int number, Func<int, (Sprite, Sprite)> getNumberAsSprite)
    {
        string numberAsString = number.ToString();

        // *number* is an unit number
        if (numberAsString.Length == 1)
        {
            unitNumberContainer.SetActive(true);
            tensNumberContainer.SetActive(false);

            (Sprite _, Sprite unitNumberSprite) = getNumberAsSprite(number);
            unitNumberImage.sprite = unitNumberSprite;
        }
        // *number* is a tens number
        else if (numberAsString.Length > 1)
        {
            tensNumberContainer.SetActive(true);
            unitNumberContainer.SetActive(false);

            (Sprite tensNumberSprite, Sprite unitNumberSprite) = getNumberAsSprite(number);
            tensTensNumberImage.sprite = tensNumberSprite;
            tensUnitNumberImage.sprite = unitNumberSprite;
        }
    }

    public static void SetNumberAsSprites(GameObject unitNumberContainer, GameObject tensNumberContainer,
        SpriteRenderer unitNumberSpriteRenderer, SpriteRenderer tensUnitNumberSpriteRenderer, SpriteRenderer tensTensNumberSpriteRenderer,
        int number, Func<int, (Sprite, Sprite)> getNumberAsSprite)
    {
        string numberAsString = number.ToString();

        // *number* is an unit number
        if (numberAsString.Length == 1)
        {
            unitNumberContainer.SetActive(true);
            tensNumberContainer.SetActive(false);

            (Sprite _, Sprite unitNumberSprite) = getNumberAsSprite(number);
            unitNumberSpriteRenderer.sprite = unitNumberSprite;
        }
        // *number* is a tens number
        else if (numberAsString.Length > 1)
        {
            tensNumberContainer.SetActive(true);
            unitNumberContainer.SetActive(false);

            (Sprite tensNumberSprite, Sprite unitNumberSprite) = getNumberAsSprite(number);
            tensTensNumberSpriteRenderer.sprite = tensNumberSprite;
            tensUnitNumberSpriteRenderer.sprite = unitNumberSprite;
        }
    }
}
