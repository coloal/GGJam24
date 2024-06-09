using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    
    [SerializeField] private List<CombatCardTemplate> playerCards;
    [SerializeField] private GameObject combatCardPrefab;
    [SerializeField] private int maxNumberOfCardInHand = 3;

    private List<CombatCard> hand;
    private List<CombatCard> deck;
    public static DeckManager Instance;

    public DeckManager()
    {
        hand = new List<CombatCard>();
        deck = new List<CombatCard>();
    }

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

    public CombatCard GetTopCard()
    {
        if (deck.Count <= 0) return null;
        CombatCard card = deck[0];
        return card;
    }

    public CombatCard GiveTopCardToHand()
    {
        CombatCard card = GetTopCard();
        deck.Remove(card);
        if (card != null)
        {
            hand.Add(card);
        }
        return card;
    }

    public void ReturnCardToDeck(CombatCard card)
    {
        hand.Remove(card);
        deck.Add(card);
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

    public void KillCard(CombatCard card)
    {
        hand.Remove(card);
        deck.Remove(card);
    }

    
    public void FinishCombat()
    {
        hand.Clear();
        deck.Clear();
    }

    public void StartCombat()
    {
        playerCards.ForEach(cardTemplate => {
            CombatCard card = Instantiate(combatCardPrefab)?.GetComponent<CombatCard>();
            card.gameObject.SetActive(false);
            card.SetDataCard(cardTemplate);
            deck.Add(card);
        });
    }

    public int GetMaxNumberOfCardsInHand()
    {
        return maxNumberOfCardInHand;
    }

    public List<CombatCard> GetCardsInHand()
    {
        return hand;
    }

    public int GetNumberOfCardsInDeck()
    {
        return deck.Count;
    }
}
