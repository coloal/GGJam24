using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatCard : CombatCard
{
    // DEBUG
    protected override (Color, Color) healthBarColors => (new Color(0.27f, 0.62f, 0.83f), new Color(0.03f, 0.20f, 0.30f));

    protected override void SetCardBackgroundSprite(CombatCardTemplate combatCardTemplate)
    {
        if (visualComposerComponent)
        {
            backgroundCombatSprite.sprite = visualComposerComponent.GetCardBackgroundUnknownSprite();
        }
    }

    protected override void SetCardCharacterSprite(CombatCardTemplate combatCardTemplate)
    {
        if (visualComposerComponent)
        {
            characterSprite.sprite = visualComposerComponent.GetCardCharacterUnknownSprite(combatCardTemplate);
        }
    }

    protected override (Sprite, Sprite) GetCardStatsSprites(int stat)
    {
        if (visualComposerComponent)
        {
            return visualComposerComponent.GetUnknownStatsNumberAsSprites(stat);
        }

        return (null, null);
    }

    protected override (Sprite, Sprite) GetCardHpStatsSprites(int stat)
    {
        if (visualComposerComponent)
        {
            return visualComposerComponent.GetUnknownHpStatsNumberAsSprites(stat);
        }

        return (null, null);
    }

    protected override void SetUpEnergyPoints(int energyPoints)
    {
        // This method does nothing for an EnemyCombatCard
    }

    protected override Sprite GetCardHpSeparatorSprite()
    {
        if (visualComposerComponent)
        {
            return visualComposerComponent.GetCardHpSeparatorUnknownSprite();
        }

        return null;
    }
}
