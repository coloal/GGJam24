using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyDeckManager : EnemyDeckManager
{
    public override CombatCard GetCardFromDeck()
    {
        return GetTopCardFromDeck();
    }

    CombatCard GetTopCardFromDeck()
    {
        if (deck.Count <= 0) return null;
        CombatCard card = deck[0];

        return card;
    }
}
