using UnityEngine;

[CreateAssetMenu(fileName = "StoryCardTemplate", menuName = "Story Card Template")]
public class StoryCardTemplate : ScriptableObject
{
    //Information of the card
    public string NameOfCard;
    public string Text;

    //Choices text
    public string LeftText;
    public string RightText;

    //Card sprite
    public Sprite CharacterSprite;
}
