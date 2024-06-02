using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CombatV2Manager : MonoBehaviour
{
    public struct CombatContext
    {
        GameObject enemyCardsContainer;
        GameObject playerHandContainer;
        GameObject playerDeck;
        GameObject playerOnCombatCard;
        GameObject enemyOnCombatCard;

        public CombatContext(GameObject enemyCardsContainer,
            GameObject playerHandContainer,
            GameObject playerDeck,
            GameObject playerOnCombatCard,
            GameObject enemyOnCombatCard)
        {
            this.enemyCardsContainer = enemyCardsContainer;
            this.playerHandContainer = playerHandContainer;
            this.playerDeck = playerDeck;
            this.playerOnCombatCard = playerOnCombatCard;
            this.enemyOnCombatCard = enemyOnCombatCard;
        }
    }

    [Header("Board configurations")]
    [SerializeField] private GameObject EnemyCardsContainer;
    [SerializeField] private GameObject PlayerHandContainer;
    [SerializeField] private GameObject PlayerDeck;
    [SerializeField] private GameObject PlayerOnCombatCard;
    [SerializeField] private GameObject EnemyOnCombatCard;

    private CombatContext combatContext;

    void Start()
    {
        SetUpManagers();
        InitCombarContext();
        ProcessCombat();
    }

    private void InitCombarContext()
    {
        combatContext = new CombatContext(
            EnemyCardsContainer,
            PlayerHandContainer,
            PlayerDeck,
            PlayerOnCombatCard,
            EnemyOnCombatCard
        );
    }

    private void ProcessCombat()
    {
        CombatState currentCombatState = new StartCombatState();
        while (!(currentCombatState is ResultWinState) 
            || !(currentCombatState is ResultLoseState))
        {
            currentCombatState = currentCombatState.Process(combatContext);
        }
    }

    private void SetUpManagers()
    {
        // Set up DeckManager
        Debug.Log("Managers, set uped!");
    }
}
