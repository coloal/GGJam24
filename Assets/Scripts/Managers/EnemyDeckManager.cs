using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckManager : MonoBehaviour
{
    [SerializeField] private GameObject combatCardPrefab;

    private EnemyTemplate template;
    private List<CombatCard> cardsInUse;
    private List<CombatCard> deck;

    void Awake()
    {
        cardsInUse = new List<CombatCard>();
        deck = new List<CombatCard>();
    }
    
    public CombatCard SelectRandomCard()
    {
        CombatCard selectedCard = deck[Random.Range(0, cardsInUse.Count)];
        cardsInUse.Add(selectedCard);
        deck.Remove(selectedCard);

        return selectedCard;
    }

    public void KillCard(CombatCard card) 
    {
        cardsInUse.Remove(card);
        deck.Remove(card);
    }

    public void StartCombat(EnemyTemplate template)
    {
        this.template = template;
        template.CombatCards.ForEach(handTemplate =>
        {
            CombatCard card = Instantiate(combatCardPrefab)?.GetComponent<CombatCard>();
            card.gameObject.SetActive(false);
            card.SetDataCard(handTemplate);

            deck.Add(card);
        });
    }

    public CombatCardTemplate SelectCardToSave(int cardIndex)
    {
        return template.CombatCards[cardIndex];
    }

    public void ReturnCardToDeck(CombatCard cardToReturnToDeck)
    {
        cardsInUse.Remove(cardToReturnToDeck);
        deck.Add(cardToReturnToDeck);
    }

    public int GetNumberOfCardsInDeck()
    {
        return deck.Count;
    }

    public List<CombatCardTemplate> GetAllEnemyCards()
    {
        return template.CombatCards;
    }
}
