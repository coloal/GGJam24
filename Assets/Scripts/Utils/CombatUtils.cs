using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public static class CombatUtils
{
    public const int NUMBER_OF_ENEMY_CARDS_TYPE_HINTS_ROWS = 2;

    public static void ForEachCardInCardsContainer(List<Transform> cardsContainers, Action<GameObject> withCardInContainer)
    {
        foreach (Transform cardContainer in cardsContainers)
        {
            // Do only when the card container has a card
            if (cardContainer.childCount > 0)
            {
                GameObject cardInContainer = cardContainer.GetChild(0).gameObject;
                withCardInContainer(cardInContainer);
            }
        }
    }

    public static async Task ForEachCardInCardsContainerTask(List<Transform> cardsContainers, Func<GameObject, Task> withCardInContainer)
    {
        foreach (Transform cardContainer in cardsContainers)
        {
            // Do only when the card container has a card
            if (cardContainer.childCount > 0)
            {
                GameObject cardInContainer = cardContainer.GetChild(0).gameObject;
                await withCardInContainer(cardInContainer);
            }
        }
    }

    public static void ProcessNextStateAfterSeconds(CombatState nextState, float seconds)
    {
        GameUtils.CreateTemporizer(() =>
        {
            CombatSceneManager.Instance.ProvideCombatManager().ProcessCombat(nextState);
        }, seconds, CombatSceneManager.Instance);
    }
}
