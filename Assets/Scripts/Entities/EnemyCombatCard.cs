using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatCard : CombatCard
{
    // (Color1, Color2)
    // Color1: Health bar fill color
    // color2: Health bar background color (empty color)
    protected override (Color, Color) healthBarColors { 
        get {
            return (GameUtils.GetNormalizedColor(71, 159, 214), GameUtils.GetNormalizedColor(194, 232, 255));
        }
    }

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
