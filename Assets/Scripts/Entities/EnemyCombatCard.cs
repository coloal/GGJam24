using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatCard : CombatCard
{
    // DEBUG
    protected override Color healthBarColor => Color.green;

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
}
