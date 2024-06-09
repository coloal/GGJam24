using UnityEngine;
using UnityEngine.UI;

public class CombatCard : MonoBehaviour
{
    [Header("Visual configurations")]
    [SerializeField] protected Image cardSprite;
    [SerializeField] private GameObject inactiveOverlay;

    protected CombatTypes combatType;
    protected CombatCardVisualComposerComponent visualComposerComponent;
    protected void SetCardSprite(CombatCardTemplate combatCardTemplate)
    {
        cardSprite.sprite = combatCardTemplate.CharacterSprite;
    }

    void Awake()
    {
        visualComposerComponent = GetComponent<CombatCardVisualComposerComponent>();
    }

    public void SetDataCard(CombatCardTemplate dataCard)
    {
        combatType = dataCard.CombatType;
        SetCardSprite(dataCard);
    }

  

    public CombatTypes GetCombatType()
    {
        return combatType;
    }

    public void SetInactiveOverlayActivation(bool isActive)
    {
        inactiveOverlay.SetActive(isActive);
    }
}
