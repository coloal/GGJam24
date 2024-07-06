using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField]
    private int deckSize = 8;
    private List<CombatCardTemplate> cardsVault;
    private List<CombatCardTemplate> deck;

    

    [Header("Debug")]
    [SerializeField] List<CombatCardTemplate> debugDeck;

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

        Init();
    }

    void Init()
    {
        cardsVault = new List<CombatCardTemplate>();
        deck = new List<CombatCardTemplate>();
    }

    public List<CombatCardTemplate> GetDeckCopy()
    {
        List<CombatCardTemplate> clonedDeck = new List<CombatCardTemplate>();
        

        /*
         deck.ForEach((combatCardData) => 
         {
             clonedDeck.Add(combatCardData);
         });
        
        //DEBUG PURPOSES ONLY
        /*/
        debugDeck.ForEach((combatCardData) => 
        {
            clonedDeck.Add(combatCardData);
        });
        /**/
        return clonedDeck;
    }

    public void AddCombatCardToVault(CombatCardTemplate combatCard)
    {
        cardsVault.Add(combatCard);
        if(deck.Count < deckSize) {
            deck.Add(combatCard);

            //DEBUG PURPOSES ONLY
            debugDeck.Add(combatCard);
        }
    }

    public int GetNumberOfCardsInDeck()
    {
        // return deck.Count;
        return debugDeck.Count;
    }

    public void RestInventory()
    {
        cardsVault.Clear();
        deck.Clear();
    }
}
