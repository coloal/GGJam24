using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatCard : CombatCard
{
    [Header("Energy stat")]
    [SerializeField] private Image[] energyCellsImages;

    // (Color1, Color2)
    // Color1: Health bar fill color
    // color2: Health bar background color (empty color)
    protected override (Color, Color) healthBarColors {
        get {
            switch (combatType)
            {
                case CombatTypes.Influence:
                    return (GameUtils.GetNormalizedColor(215, 71, 188), GameUtils.GetNormalizedColor(255, 193, 212));
                case CombatTypes.Money:
                    return (GameUtils.GetNormalizedColor(71, 215, 93), GameUtils.GetNormalizedColor(193, 255, 234));
                case CombatTypes.Violence:
                    return (GameUtils.GetNormalizedColor(235, 64, 52), GameUtils.GetNormalizedColor(255, 232, 193));
            }

            return (new Color(0.27f, 0.62f, 0.83f), new Color(0.03f, 0.20f, 0.30f));
        }
    }

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
        int energyCellsImagesCyclesToGo = initialEnergy / energyCellsImages.Length;
        int currentCycle = 0;
        int energyPointsToFill = 0;

        // Deactivate all energy cells for not showing the ones that are not filled
        for (int i = 0; i < energyCellsImages.Length; i++)
        {
            energyCellsImages[i].gameObject.SetActive(false);
            energyCellsImages[i].sprite = null;
        }

        while (currentCycle <= energyCellsImagesCyclesToGo)
        {
            for (int i = 0; (energyPointsToFill < energyPoints) && (i < energyCellsImages.Length); i++)
            {
                energyCellsImages[i].sprite =
                    visualComposerComponent.GetEnergyCellStatSprite(combatType, currentCycle);
                energyPointsToFill++;
            }
            currentCycle++;
        }

        // Reactivate all valid energy cells
        for (int i = 0; i < energyCellsImages.Length; i++)
        {
            energyCellsImages[i].gameObject.SetActive(energyCellsImages[i].sprite != null);
        }
    }
}
