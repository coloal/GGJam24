using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseDeckManager : MonoBehaviour
{
    protected List<CombatCard> hand;
    protected List<CombatCard> deck;
    protected List<CombatCard> tieZone;

    protected BaseDeckManager()
    {
        hand = new List<CombatCard>();
        deck = new List<CombatCard>();
        tieZone = new List<CombatCard>();
    }

    public abstract CombatCard GetCardFromDeck();

    public CombatCard DrawCardFromDeckToHand()
    {
        CombatCard cardToDraw = GetCardFromDeck();
        deck.Remove(cardToDraw);
        hand.Add(cardToDraw);

        return cardToDraw;
    }

    public void ReturnCardFromHandToDeck(CombatCard combatCard)
    {
        hand.Remove(combatCard);
        deck.Add(combatCard);
    }

    public void ReturnCardFromTieZoneToDeck(CombatCard combatCard)
    {
        tieZone.Remove(combatCard);
        deck.Add(combatCard);
    }

    public void AddCardToDeck(CombatCard combatCard)
    {
        deck.Add(combatCard);
    }

    public void AddCardFromHandToTieZone(CombatCard combatCard)
    {
        hand.Remove(combatCard);
        tieZone.Add(combatCard);
    }

    public void ShuffleDeck()
    {
        List<CombatCard> auxList = new List<CombatCard>(deck);
        deck.Clear();
        while (auxList.Any())
        {
            int idx = Random.Range(0, auxList.Count - 1);
            deck.Add(auxList[idx]);
            auxList.RemoveAt(idx);
        }
    }

    public void DestroyCard(CombatCard combatCard)
    {
        hand.Remove(combatCard);
        deck.Remove(combatCard);
        tieZone.Remove(combatCard);
    }

    public List<CombatCard> GetCardsInHand()
    {
        return hand;
    }

    public List<CombatCard> GetCardsInDeck()
    {
        return deck;
    }

    public List<CombatCardTemplate> GetAllDeckCardsData()
    {
        return deck.Select((combatCard) => combatCard.GetCardData()).ToList();
    }

    public List<CombatCard> GetCardsInTieZone()
    {
        return tieZone;
    }

    public int GetNumberOfCardsInDeck()
    {
        return deck.Count;
    }

    public int GetNumberOfCardsInHand()
    {
        return hand.Count;
    }

    public int GetNumberOfCardsInTieZone()
    {
        return tieZone.Count;
    }

}
