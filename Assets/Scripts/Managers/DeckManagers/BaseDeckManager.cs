using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseDeckManager : MonoBehaviour
{
    protected List<CombatCard> hand;
    protected List<CombatCard> deck;
    protected List<CombatCard> tieZone;

    public delegate void OnCardsContainerStateUpdate(List<CombatCard> deck);
    public event OnCardsContainerStateUpdate onDeckStateUpdate;
    public event OnCardsContainerStateUpdate onHandStateUpdate;
    public event OnCardsContainerStateUpdate onTieZoneStateUpdate;

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
        onDeckStateUpdate?.Invoke(deck);
        
        hand.Add(cardToDraw);
        onHandStateUpdate?.Invoke(hand);

        return cardToDraw;
    }

    public void PutCardFromHandToCombatZone(CombatCard combatCard)
    {
        hand.Remove(combatCard);
        onHandStateUpdate?.Invoke(hand);
    }

    public void ReturnCardFromTieZoneToDeck(CombatCard combatCard)
    {
        tieZone.Remove(combatCard);
        onTieZoneStateUpdate?.Invoke(tieZone);
        deck.Add(combatCard);
        onDeckStateUpdate?.Invoke(deck);
    }

    public void AddCardToDeck(CombatCard combatCard)
    {
        deck.Add(combatCard);
        onDeckStateUpdate?.Invoke(deck);
    }

    public void AddCardToTieZone(CombatCard combatCard)
    {
        tieZone.Add(combatCard);
        onTieZoneStateUpdate?.Invoke(tieZone);
    }

    public void ShuffleDeck()
    {
        List<CombatCard> auxList = new List<CombatCard>(deck);
        deck.Clear();
        while (auxList.Any())
        {
            int idx = UnityEngine.Random.Range(0, auxList.Count);
            deck.Add(auxList[idx]);
            auxList.RemoveAt(idx);
        }

        onDeckStateUpdate?.Invoke(deck);
    }

    public void DestroyCard(CombatCard combatCard)
    {
        hand.Remove(combatCard);
        onHandStateUpdate?.Invoke(hand);

        deck.Remove(combatCard);
        onDeckStateUpdate?.Invoke(deck);

        tieZone.Remove(combatCard);
        onTieZoneStateUpdate?.Invoke(tieZone);
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
