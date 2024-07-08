using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckManager : BaseDeckManager
{
    public override CombatCard GetCardFromDeck()
    {
        return SelectARandomCardFromDeck();
    }

    CombatCard SelectARandomCardFromDeck()
    {
        CombatCard selectedCard = deck[Random.Range(0, deck.Count)];

        return selectedCard;
    }
}
