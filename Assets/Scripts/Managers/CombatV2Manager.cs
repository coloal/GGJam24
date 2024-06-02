using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CombatV2Manager : MonoBehaviour
{
    public struct CombatContext
    {
        public GameObject enemyCardsContainer;
        public Transform enemyCardsContainerFinalPosition;
        public GameObject playerHandContainer;
        public GameObject playerDeck;
        public GameObject playerOnCombatCard;
        public GameObject enemyOnCombatCard;

        public CombatContext(GameObject enemyCardsContainer,
            Transform enemyCardsContainerFinalPosition,
            GameObject playerHandContainer,
            GameObject playerDeck,
            GameObject playerOnCombatCard,
            GameObject enemyOnCombatCard)
        {
            this.enemyCardsContainer = enemyCardsContainer;
            this.enemyCardsContainerFinalPosition = enemyCardsContainerFinalPosition;
            this.playerHandContainer = playerHandContainer;
            this.playerDeck = playerDeck;
            this.playerOnCombatCard = playerOnCombatCard;
            this.enemyOnCombatCard = enemyOnCombatCard;
        }
    }

    [Header("Board configurations")]
    [SerializeField] private GameObject EnemyCardsContainer;
    [SerializeField] private Transform EnemyCardsContainerFinalPosition;
    [SerializeField] private GameObject PlayerHandContainer;
    [SerializeField] private GameObject PlayerDeck;
    [SerializeField] private GameObject PlayerOnCombatCard;
    [SerializeField] private GameObject EnemyOnCombatCard;

    private CombatContext combatContext;

    void Start()
    {
        SetUpManagers();
        InitCombatContext();
        ProcessCombat(new StartCombatState());
    }

    private void InitCombatContext()
    {
        combatContext = new CombatContext(
            EnemyCardsContainer,
            EnemyCardsContainerFinalPosition,
            PlayerHandContainer,
            PlayerDeck,
            PlayerOnCombatCard,
            EnemyOnCombatCard
        );
    }

    public void ProcessCombat(CombatState combatState)
    {
        combatState.Process(combatContext);
    }

    private void SetUpManagers()
    {
        // Set up DeckManager
        Debug.Log("Managers, set uped!");
    }
}
