using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public struct CombatContext
    {
        public GameObject enemyCardsHintRow0;
        public GameObject enemyCardsHintRow1;
        public GameObject enemyCardsPickUpRow0;
        public GameObject enemyCardsPickUpRow1;
        public Transform playerHandContainer;
        public DeckBehaviourComponent playerDeck;
        public GameObject playerOnCombatCard;
        public Transform playerOnCombatCardTransform;
        public GameObject enemyOnCombatCard;
        public Transform enemyOnCombatCardOriginalPosition;
        public Transform enemyOnCombatCardFinalPosition;
        public GameObject combatContainer;
        public Transform playerTieZone;
        public Transform enemyTieZone; 

        public CombatContext(GameObject enemyCardsHintRow0,
            GameObject enemyCardsHintRow1,
            GameObject enemyCardsPickUpRow0,
            GameObject enemyCardsPickUpRow1,
            Transform playerHandContainer,
            DeckBehaviourComponent playerDeck,
            Transform playerOnCombatCardTransform,
            Transform enemyOnCombatCardOriginalPosition,
            Transform enemyOnCombatCardFinalPosition,
            GameObject combatContainer,
            Transform playerTieZone,
            Transform enemyTieZone)
        {
            this.enemyCardsHintRow0 = enemyCardsHintRow0;
            this.enemyCardsHintRow1 = enemyCardsHintRow1;
            this.enemyCardsPickUpRow0 = enemyCardsPickUpRow0;
            this.enemyCardsPickUpRow1 = enemyCardsPickUpRow1;
            this.playerHandContainer = playerHandContainer;
            this.playerDeck = playerDeck;
            this.playerOnCombatCardTransform = playerOnCombatCardTransform;
            this.enemyOnCombatCardOriginalPosition = enemyOnCombatCardOriginalPosition;
            this.enemyOnCombatCardFinalPosition = enemyOnCombatCardFinalPosition;
            this.combatContainer = combatContainer;
            this.playerTieZone = playerTieZone;
            this.enemyTieZone = enemyTieZone;

            this.playerOnCombatCard = null;
            this.enemyOnCombatCard = null;
        }

        public List<Transform> GetPlayerCardInHandContainers()
        {
            return GetCardContainers(playerHandContainer);
        }

        public List<Transform> GetPlayerCardInTieZoneContainers()
        {
            return GetCardContainers(playerTieZone);
        }

        public List<Transform> GetEnemyCardInTieZoneContainers()
        {
            return GetCardContainers(enemyTieZone);
        }

        List<Transform> GetCardContainers(Transform cardContainersParentTransform)
        {
            List<Transform> cardContainers = new List<Transform>();
            foreach (Transform cardContainer in cardContainersParentTransform)
            {
                cardContainers.Add(cardContainer);
            }

            return cardContainers;
        }
    }

    [Header("Board configurations")]
    [SerializeField] private Image enemyCharacterImage;
    [SerializeField] private GameObject enemyCardsHintRow0;
    [SerializeField] private GameObject enemyCardsHintRow1;
    [SerializeField] private GameObject enemyCardsPickUpRow0;
    [SerializeField] private GameObject enemyCardsPickUpRow1;
    [SerializeField] private CombatTypeHintComponent combatTypeHintPrefab;
    [SerializeField] private GameObject combatCardPrefab;
    [SerializeField] private GameObject emptyCardDummy;

    [Header("Combat zone")]
    [SerializeField] private GameObject combatContainer;
    [SerializeField] private Transform playerOnCombatCardTransform;
    [SerializeField] private Transform enemyOnCombatCardOriginalPosition;
    [SerializeField] private Transform enemyOnCombatCardFinalPosition;
    [SerializeField] private Transform playerTieZone;
    [SerializeField] private Transform enemyTieZone;

    [Header("Player deck")]
    [SerializeField] private DeckBehaviourComponent playerDeck;
    
    [Header("Player hand")]
    [SerializeField] private Transform playerHandContainer;

    [Header("Enemy deck")]
    [SerializeField] private int maxAllowedEnemyCards = 8;

    [Header("Time for next state configurations")]
    [SerializeField] public float timeForPresentEnemyCards = 0.5f;
    [SerializeField] public float timeForPresentPlayerCards = 0.5f;
    [SerializeField] public float timeForPickEnemyCard = 0.5f;
    [SerializeField] public float timeForPickPlayerCard = 0.5f;
    [SerializeField] public float timeForShowCards = 0.5f;
    [SerializeField] public float timeForResolveCombat = 0.5f;
    [SerializeField] public float timeForNextCombatRound = 0.5f;
    [SerializeField] public float timeForDrawRound = 0.5f;
    [SerializeField] public float timeForTossCoin = 0.5f;
    [SerializeField] public float timeForCombatResultsRound = 0.5f;


    private CombatContext combatContext;

    void Start()
    {
        SetUpManagers();
        InitEnemyInfo();
        InitCombatContext();
        ProcessCombat(new StartCombatState());
    }

    void InitEnemyInfo()
    {
        enemyCharacterImage.sprite = CombatSceneManager.Instance.ProvideEnemyData().characterSprite;
    }

    void InitCombatContext()
    {
        combatContext = new CombatContext(
            enemyCardsHintRow0,
            enemyCardsHintRow1,
            enemyCardsPickUpRow0,
            enemyCardsPickUpRow1,
            playerHandContainer,
            playerDeck,
            playerOnCombatCardTransform,
            enemyOnCombatCardOriginalPosition,
            enemyOnCombatCardFinalPosition,
            combatContainer,
            playerTieZone,
            enemyTieZone
        );
    }

    public void ProcessCombat(CombatState combatState)
    {
        combatState.Process(combatContext);
    }

    void SetUpManagers()
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
