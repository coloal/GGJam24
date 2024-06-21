using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDeckManager : BaseDeckManager
{
    // Maybe this method requieres CombatCard to hold a reference to its CombatCardTemplate object data
    [SerializeField] private List<CombatCardTemplate> playerCardsData;
    [SerializeField] private int maxNumberOfCardInHand = 3;
    public static PlayerDeckManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

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

    public override List<CombatCardTemplate> GetAllCardsData()
    {
        return playerCardsData;
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
