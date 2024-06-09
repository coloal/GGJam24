using UnityEngine;
using UnityEngine.UI;

public class CombatTypeHintComponent : MonoBehaviour
{
    public void SetCombatTypeHint(CombatTypes combatType)
    {
        Image imageComponent = GetComponent<Image>();
        if (imageComponent != null)
        {
            switch (combatType)
            {
                case CombatTypes.Money:
                    imageComponent.color = Color.green;
                    break;
                case CombatTypes.Influence:
                    imageComponent.color = Color.blue;
                    break;
                case CombatTypes.Violence:
                    imageComponent.color = Color.red;
                    break;
                default:
                    break;
            }
        }
    }
}
