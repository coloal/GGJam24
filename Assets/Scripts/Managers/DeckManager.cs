using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private GameObject CombatCardPrefab;
    private List<CombatCardTemplate> PlayerCards;
    private List<CombatCard> Hand;
    private List<CombatCard> Deck;
  
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CombatCard GetTopCard()
    {
        if (Deck.Count <= 0) return null;
        CombatCard card = Deck[0];
        return card;
    }

    public CombatCard GiveTopCardToHand()
    {
        CombatCard card = GetTopCard();
        Deck.Remove(card);
        if (card != null)
        {
            Hand.Add(card);
        }
        return card;
    }

    public void ReturnCardToDeck(CombatCard card)
    {
        Hand.Remove(card);
        Deck.Add(card);
    }

    public void ShuffleDeck()
    {
        List<CombatCard> auxList = new List<CombatCard>(Deck);
        Deck.Clear();
        while (auxList.Any())
        {
            int idx = Random.Range(0, auxList.Count - 1);
            Deck.Add(auxList[idx]);
            auxList.RemoveAt(idx);
        }
    }

    public void KillCard(CombatCard card)
    {
        Hand.Remove(card);
    }

    
    public void FinishCombat()
    {
        Hand.Clear();
        Deck.Clear();
    }

    public void StartCombat()
    {
        PlayerCards.ForEach(cardTemplate => {
            CombatCard card = Instantiate(CombatCardPrefab)?.GetComponent<CombatCard>();
            card.gameObject.SetActive(false);
            card.SetDataCard(cardTemplate);
            Hand.Add(card);
        });
    }
}
