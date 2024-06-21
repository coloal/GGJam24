using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckManager : BaseDeckManager
{
    EnemyTemplate enemyData;

    public override CombatCard GetCardFromDeck()
    {
        return SelectARandomCardFromDeck();
    }

    CombatCard SelectARandomCardFromDeck()
    {
        CombatCard selectedCard = deck[Random.Range(0, hand.Count)];

        return selectedCard;
    }

    public override List<CombatCardTemplate> GetAllCardsData()
    {
        return enemyData.CombatCards;
    }
}
