using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{

    /*[SerializeField]
    private CardTemplate DataCard;*/

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

    //Incrementos/Decrementos
    public int InfluenceStat;
    public int MoneyStat;
    public int ViolenceStat;

    public List<HitManTypes> ListHitmanTypes;
    public HitmanInfo Contable;
    public HitmanInfo Maton;
    public HitmanInfo Comisario;

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

    public void SetDataCard(CardTemplate DataCard) 
    {
        //Set informaci�n del DataCard
        NameOfCard = DataCard.NameOfCard;

        InfluenceStat = DataCard.InfluenceStat;
        MoneyStat = DataCard.MoneyStat;
        ViolenceStat = DataCard.ViolenceStat;

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

        ListHitmanTypes = DataCard.ListHitmanTypes;

        Contable = DataCard.Contable;
        Maton = DataCard.Maton;
        Comisario = DataCard.Comisario;
}

}
