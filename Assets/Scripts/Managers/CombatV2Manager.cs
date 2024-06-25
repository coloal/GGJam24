using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class CombatV2Manager : MonoBehaviour
{
    public struct CombatContext
    {
        public GameObject enemyCardsHintRow0;
        public GameObject enemyCardsHintRow1;
        public Transform enemyCardsCombatTypeHintsContainerFinalPosition;
        public GameObject playerHandContainer;
        public DeckBehaviourComponent playerDeck;
        public GameObject playerOnCombatCard;
        public GameObject playerOnCombatCardFinalPosition;
        public GameObject enemyOnCombatCard;
        public GameObject enemyOnCombatCardFinalPosition;
        public GameObject combatContainer;
        public Transform playerTieZone;
        public Transform enemyTieZone; 
        public Transform playerCardInHandPosition0;
        public Transform playerCardInHandPosition1;
        public Transform playerCardInHandPosition2;

        public CombatContext(GameObject enemyCardsHintRow0,
            GameObject enemyCardsHintRow1,
            Transform enemyCardsCombatTypeHintsContainerFinalPosition,
            GameObject playerHandContainer,
            DeckBehaviourComponent playerDeck,
            GameObject playerOnCombatCardFinalPosition,
            GameObject enemyOnCombatCardFinalPosition,
            GameObject combatContainer,
            Transform playerTieZone,
            Transform enemyTieZone,
            Transform playerCardInHandPosition0,
            Transform playerCardInHandPosition1,
            Transform playerCardInHandPosition2)
        {
            this.enemyCardsHintRow0 = enemyCardsHintRow0;
            this.enemyCardsHintRow1 = enemyCardsHintRow1;
            this.enemyCardsCombatTypeHintsContainerFinalPosition = enemyCardsCombatTypeHintsContainerFinalPosition;
            this.playerHandContainer = playerHandContainer;
            this.playerDeck = playerDeck;
            this.playerOnCombatCardFinalPosition = playerOnCombatCardFinalPosition;
            this.enemyOnCombatCardFinalPosition = enemyOnCombatCardFinalPosition;
            this.combatContainer = combatContainer;
            this.playerTieZone = playerTieZone;
            this.enemyTieZone = enemyTieZone;
            this.playerCardInHandPosition0 = playerCardInHandPosition0;
            this.playerCardInHandPosition1 = playerCardInHandPosition1;
            this.playerCardInHandPosition2 = playerCardInHandPosition2;

            this.playerOnCombatCard = null;
            this.enemyOnCombatCard = null;
        }
    }

    [Header("Board configurations")]
    [SerializeField] private GameObject enemyCardsHintRow0;
    [SerializeField] private GameObject enemyCardsHintRow1;
    [SerializeField] private Transform enemyCardsCombatTypeHintsContainerFinalPosition;
    [SerializeField] private GameObject playerOnCombatCardFinalPosition;
    [SerializeField] private GameObject enemyOnCombatCardFinalPosition;
    [SerializeField] private GameObject combatContainer;
    [SerializeField] private CombatTypeHintComponent combatTypeHintPrefab;
    [SerializeField] private GameObject combatCardPrefab;
    [SerializeField] private GameObject emptyCardDummy;
    [SerializeField] private Transform playerTieZone;
    [SerializeField] private Transform enemyTieZone;

    [Header("Player deck")]
    [SerializeField] private DeckBehaviourComponent playerDeck;
    
    [Header("Player hand")]
    [SerializeField] private GameObject playerHandContainer;
    [SerializeField] private Transform playerCardInHandPosition0;
    [SerializeField] private Transform playerCardInHandPosition1;
    [SerializeField] private Transform playerCardInHandPosition2;

    [Header("Enemy deck")]
    [SerializeField] private int maxAllowedEnemyCards = 8;


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
            enemyCardsHintRow0,
            enemyCardsHintRow1,
            enemyCardsCombatTypeHintsContainerFinalPosition,
            playerHandContainer,
            playerDeck,
            playerOnCombatCardFinalPosition,
            enemyOnCombatCardFinalPosition,
            combatContainer,
            playerTieZone,
            enemyTieZone,
            playerCardInHandPosition0,
            playerCardInHandPosition1,
            playerCardInHandPosition2
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

    public void OverwriteCombatContext(CombatContext newCombatContext)
    {
        combatContext = newCombatContext;
    }

    public GameObject InstantiateCombatTypeHintGameObject()
    {
        return Instantiate(combatTypeHintPrefab).gameObject;
    }

    public GameObject InstantiateCombatCardGameObject()
    {
        return Instantiate(combatCardPrefab).gameObject;
    }

    public GameObject InstantiateEmptyCardDummyGameObject()
    {
        return Instantiate(emptyCardDummy).gameObject;
    }

    public int GetMaxAllowedEnemyCards()
    {
        return maxAllowedEnemyCards;
    }
}
