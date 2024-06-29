using UnityEngine;
using UnityEngine.UI;

public class CombatTypeHintComponent : MonoBehaviour
{
    [SerializeField] private Image cardBackImage;

    [Header("Type hints visual assets")]
    [SerializeField] private Sprite influenceTypeHintSprite;
    [SerializeField] private Sprite moneyTypeHintSprite;
    [SerializeField] private Sprite violenceTypeHintSprite;

    public void SetCombatTypeHint(CombatTypes combatType)
    {
        switch (combatType)
        {
            case CombatTypes.Influence:
                cardBackImage.sprite = influenceTypeHintSprite;
                break;
            case CombatTypes.Money:
                cardBackImage.sprite = moneyTypeHintSprite;
                break;
            case CombatTypes.Violence:
                cardBackImage.sprite = violenceTypeHintSprite;
                break;
        }
    }
}
