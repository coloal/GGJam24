using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatCard : CombatCard
{
    [Header("Energy stat")]
    [SerializeField] private Image[] energyCellsImages;

    protected override Color healthBarColor => Color.yellow;

    protected override Sprite GetCardHpSeparatorSprite()
    {
        if (visualComposerComponent)
        {
            return visualComposerComponent.GetCardHpSeparatorSprite(combatType);
        }

        return null;
    }

    protected override (Sprite, Sprite) GetCardHpStatsSprites(int stat)
    {
        if (visualComposerComponent)
        {
            return visualComposerComponent.GetHpStatsNumberAsSprites(combatType, stat);
        }

        return (null, null);
    }

    protected override (Sprite, Sprite) GetCardStatsSprites(int stat)
    {
        if (visualComposerComponent)
        {
            return visualComposerComponent.GetStatsNumberAsSprites(combatType, stat);
        }

        return (null, null);
    }

    protected override void SetCardBackgroundSprite(CombatCardTemplate combatCardTemplate)
    {
        if (visualComposerComponent)
        {
            backgroundCombatSprite.sprite = visualComposerComponent.GetCardBackgroundSprite(combatCardTemplate.CombatType);
        }
    }

    protected override void SetCardCharacterSprite(CombatCardTemplate combatCardTemplate)
    {
        if (visualComposerComponent)
        {
            characterSprite.sprite = visualComposerComponent.GetCardCharacterSprite(combatCardTemplate);
        }
    }

    protected override void SetUpEnergyPoints(int energyPoints)
    {
        if (visualComposerComponent)
        {
            for (int i = 0; i < energyCellsImages.Length; i++)
            {
                energyCellsImages[i].sprite =
                    visualComposerComponent.GetEnergyCellStatSprite(combatType, energyCellsImages.Length, i);
            }
        }
    }
}
