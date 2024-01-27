using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{

    [SerializeField]
    private CardTemplate DataCard;

    [SerializeField]
    private TextMeshPro BoxNameOfCard;

    [SerializeField]
    private TextMeshPro RightTextBox;

    [SerializeField]
    private TextMeshPro LeftTextBox;

    [SerializeField]
    private TextMeshPro DescriptionTextBox;

    [SerializeField]
    private SpriteRenderer CardSprite;

    //Information of the card
    private string NameOfCard;
    private string Name;
    private string Gen;
    private string Background;

    //Indice del chat a reproducir se debe autoincrementar al terminar con esta carta
    private int indexChats = 0;
    private List<string> Chats;

    private List<string> LeftChoices;
    private List<string> RightChoices;

    //next cards to appear
    private List<GameObject> LeftCards;
    private List<GameObject> RightCards;

    //Incrementos/Decrementos
    private int incrementBar1; //Nombre provisional pdt
    private int incrementBar2; //Nombre provisional pdt
    private int incrementBar3; //Nombre provisional pdt

    //Options text
    private string LeftText;
    private string RightText;

    // Start is called before the first frame update
    void Start()
    {

        //Set informaci�n del DataCard
        NameOfCard = DataCard.NameOfCard;
        Name = DataCard.Name;
        Gen = DataCard.Gen;
        Background = DataCard.Background;

        indexChats = DataCard.indexChats;
        Chats = DataCard.Chats;

        LeftChoices = DataCard.LeftChoices;
        RightChoices = DataCard.RightChoices;

        LeftCards = DataCard.LeftCards;
        RightCards = DataCard.RightCards;

        incrementBar1 = DataCard.incrementBar1; 
        incrementBar2 = DataCard.incrementBar2;
        incrementBar3 = DataCard.incrementBar3;

        //Mostrar texto en pantalla
        BoxNameOfCard.text = NameOfCard;

        RightTextBox.text = DataCard.RightText;
        LeftTextBox.text = DataCard.LeftText;
        RightTextBox.enabled = false;
        LeftTextBox.enabled = false;
        DescriptionTextBox.text = DataCard.Background;
        CardSprite.sprite = DataCard.CardSprite;
        DescriptionTextBox.ComputeMarginSize();
        DescriptionTextBox.autoSizeTextContainer = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowText(bool IsLeft)
    {

        if(IsLeft) LeftTextBox.enabled = true;
        else RightTextBox.enabled = true;
    }

    public void HideText(bool IsLeft)
    {
        if (IsLeft) LeftTextBox.enabled = false;
        else RightTextBox.enabled = false;
    }
}
