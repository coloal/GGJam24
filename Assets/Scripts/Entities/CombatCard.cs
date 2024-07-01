using UnityEngine;
using UnityEngine.UI;

public class CombatCard : MonoBehaviour
{
    [Header("Visual configurations")]
    [SerializeField] protected Image cardFrontSprite;
    [SerializeField] protected Image cardBackSprite;
    [SerializeField] protected GameObject cardFront;
    [SerializeField] protected GameObject cardBack;

    protected CombatTypes combatType;
    protected CombatCardVisualComposerComponent visualComposerComponent;

    protected CombatCardTemplate cardData;

    protected void SetCardSprite(CombatCardTemplate combatCardTemplate)
    {
        cardFrontSprite.sprite = combatCardTemplate.cardSprite;
    }

    void Awake()
    {
        visualComposerComponent = GetComponent<CombatCardVisualComposerComponent>();
    }

    public void SetDataCard(CombatCardTemplate cardData)
    {
        this.cardData = cardData;
        combatType = cardData.CombatType;
        SetCardSprite(cardData);
    }

    public CombatCardTemplate GetCardData()
    {
        return cardData;
    }

    public CombatTypes GetCombatType()
    {
        return combatType;
    }

    public Image GetCardFrontImage()
    {
        return cardFrontSprite;
    }

    public Image GetCardBackImage()
    {
        return cardBackSprite;
    }

    public void ActivateCardFront(bool result) 
    {
        cardFront.SetActive(result);
    }

    public void ActivateCardBack(bool result)
    {
        cardBack.SetActive(result);
    }

    public void FlipCardUpsideDown()
    {
        if (Mathf.Sign(transform.localScale.x) > 0)
        {
            transform.localScale = new Vector2(
                - transform.localScale.x,
                transform.localScale.y
            );
        }
    }
}
