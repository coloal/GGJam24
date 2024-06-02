using UnityEngine;

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
