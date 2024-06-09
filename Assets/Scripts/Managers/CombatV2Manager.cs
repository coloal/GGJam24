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
        public GameObject playerOnCombatCardFinalPosition;
        public GameObject enemyOnCombatCard;
        public GameObject enemyOnCombatCardFinalPosition;
        public EnemyTemplate enemyTemplate;
        public GameObject combatContainer;

        public CombatContext(GameObject enemyCardsContainer,
            Transform enemyCardsContainerFinalPosition,
            GameObject playerHandContainer,
            GameObject playerDeck,
            GameObject playerOnCombatCardFinalPosition,
            GameObject enemyOnCombatCard,
            GameObject enemyOnCombatCardFinalPosition,
            EnemyTemplate enemyTemplate,
            GameObject combatContainer)
        {
            this.enemyCardsContainer = enemyCardsContainer;
            this.enemyCardsContainerFinalPosition = enemyCardsContainerFinalPosition;
            this.playerHandContainer = playerHandContainer;
            this.playerDeck = playerDeck;
            this.playerOnCombatCardFinalPosition = playerOnCombatCardFinalPosition;
            this.enemyOnCombatCard = enemyOnCombatCard;
            this.enemyOnCombatCardFinalPosition = enemyOnCombatCardFinalPosition;
            this.enemyTemplate = enemyTemplate;
            this.combatContainer = combatContainer;
        }
    }

    [Header("Board configurations")]
    [SerializeField] private GameObject EnemyCardsContainer;
    [SerializeField] private Transform EnemyCardsContainerFinalPosition;
    [SerializeField] private GameObject PlayerHandContainer;
    [SerializeField] private GameObject PlayerDeck;
    [SerializeField] private GameObject PlayerOnCombatCardFinalPosition;
    [SerializeField] private GameObject EnemyOnCombatCard;
    [SerializeField] private GameObject EnemyOnCombatCardFinalPosition;
    [SerializeField] private EnemyTemplate EnemyTemplate;
    [SerializeField] private GameObject CombatContainer;

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
            PlayerOnCombatCardFinalPosition,
            EnemyOnCombatCard,
            EnemyOnCombatCardFinalPosition,
            EnemyTemplate,
            CombatContainer
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
