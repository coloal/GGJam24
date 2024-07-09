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
        public GameObject enemyCardsRow0;
        public GameObject enemyCardsRow1;
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

        public CombatContext(GameObject enemyCardsRow0,
            GameObject enemyCardsRow1,
            Transform playerHandContainer,
            DeckBehaviourComponent playerDeck,
            Transform playerOnCombatCardTransform,
            Transform enemyOnCombatCardOriginalPosition,
            Transform enemyOnCombatCardFinalPosition,
            GameObject combatContainer,
            Transform playerTieZone,
            Transform enemyTieZone)
        {
            this.enemyCardsRow0 = enemyCardsRow0;
            this.enemyCardsRow1 = enemyCardsRow1;
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

        public List<Transform> GetEnemyCards()
        {
            List<Transform> enemyCards = new List<Transform>();
            foreach (Transform enemyCard in enemyCardsRow0.transform)
            {
                enemyCards.Add(enemyCard);
            }
            foreach (Transform enemyCard in enemyCardsRow1.transform)
            {
                enemyCards.Add(enemyCard);
            }

            return enemyCards;
        }

        public void ActivateEnemyCardsContainer()
        {
            enemyCardsRow0.SetActive(true);
            enemyCardsRow1.SetActive(true);
        }

        public void DeactivateEnemyCardsContainer()
        {
            enemyCardsRow0.SetActive(false);
            enemyCardsRow1.SetActive(false);
        }

        public void CleanEnemyCardsContainer()
        {
            foreach (Transform enemyCard in enemyCardsRow0.transform)
            {
                Destroy(enemyCard.gameObject);
            }
            foreach (Transform enemyCard in enemyCardsRow1.transform)
            {
                Destroy(enemyCard.gameObject);
            }
        }
    }

    [Header("Board configurations")]
    [SerializeField] private Image enemyCharacterImage;
    [SerializeField] private GameObject enemyCardsRow0;
    [SerializeField] private GameObject enemyCardsRow1;
    [SerializeField] private CombatTypeHintComponent combatTypeHintPrefab;
    [SerializeField] private GameObject combatCardPrefab;
    [SerializeField] private GameObject emptyCardDummy;
    [SerializeField] private GameObject coinCardPrefab;
    [SerializeField] private Transform coinCardOriginTransform;
    [SerializeField] private Transform coinCardContainerTransform;
    [SerializeField] private CoinComponent coin;

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

    [HideInInspector] 
    public bool IsTaskCancellationRequested => destroyCancellationToken.IsCancellationRequested;

    void Start()
    {
        //if (GameManager.Instance.ProvideBrainManager().IsTutorial) return;
        SetUpManagers();
        InitEnemyInfo();
        InitCombatContext();
        ProcessCombat(new StartCombatState());
    }

    public void StartTutorial()
    {
        SetUpManagers();
        InitEnemyInfo();
        InitCombatContext();
        ProcessCombat(new StartTutorialState());
    }

    void InitEnemyInfo()
    {
        enemyCharacterImage.sprite = CombatSceneManager.Instance.ProvideEnemyData().characterSprite;
    }

    void InitCombatContext()
    {
        combatContext = new CombatContext(
            enemyCardsRow0,
            enemyCardsRow1,
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

    public GameObject InstantiateCoinCardGameObject()
    {
        GameObject coinCard = Instantiate(coinCardPrefab);
        coinCard.transform.SetParent(coinCardContainerTransform, worldPositionStays: false);
        coinCard.transform.position = coinCardOriginTransform.position;

        return coinCard;
    }

    public CoinComponent GetCombatCoin()
    {
        return coin;
    }
}
