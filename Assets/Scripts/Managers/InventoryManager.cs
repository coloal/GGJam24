using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<CombatCardTemplate> cardsVault;
    private List<CombatCardTemplate> deck;

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

    public List<CombatCardTemplate> GetDeckCopy()
    {
        List<CombatCardTemplate> clonedDeck = new List<CombatCardTemplate>();
        
        deck.ForEach((combatCardData) => 
        {
            clonedDeck.Add(combatCardData);
        });

        return clonedDeck;
    }
}
