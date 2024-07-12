using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDeckManager : BaseDeckManager
{
    [SerializeField] private int maxNumberOfCardInHand = 3;

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
    
    public void FinishCombat()
    {
        hand.Clear();
        deck.Clear();
    }

    public int GetMaxAllowedCardsInHand()
    {
        return maxNumberOfCardInHand;
    }
}
