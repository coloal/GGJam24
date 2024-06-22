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
        public GameObject enemyCardsCombatTypeHintsContainer;
        public Transform enemyCardsCombatTypeHintsContainerFinalPosition;
        public GameObject playerHandContainer;
        public GameObject playerDeck;
        public GameObject playerOnCombatCard;
        public GameObject playerOnCombatCardFinalPosition;
        public GameObject enemyOnCombatCard;
        public GameObject enemyOnCombatCardFinalPosition;
        public GameObject combatContainer;
        public Transform playerTieZone;
        public Transform enemyTieZone; 

        public CombatContext(GameObject enemyCardsCombatTypeHintsContainer,
            Transform enemyCardsCombatTypeHintsContainerFinalPosition,
            GameObject playerHandContainer,
            GameObject playerDeck,
            GameObject playerOnCombatCardFinalPosition,
            GameObject enemyOnCombatCardFinalPosition,
            GameObject combatContainer,
            Transform playerTieZone,
            Transform enemyTieZone)
        {
            this.enemyCardsCombatTypeHintsContainer = enemyCardsCombatTypeHintsContainer;
            this.enemyCardsCombatTypeHintsContainerFinalPosition = enemyCardsCombatTypeHintsContainerFinalPosition;
            this.playerHandContainer = playerHandContainer;
            this.playerDeck = playerDeck;
            this.playerOnCombatCardFinalPosition = playerOnCombatCardFinalPosition;
            this.enemyOnCombatCardFinalPosition = enemyOnCombatCardFinalPosition;
            this.combatContainer = combatContainer;
            this.playerTieZone = playerTieZone;
            this.enemyTieZone = enemyTieZone;

            this.playerOnCombatCard = null;
            this.enemyOnCombatCard = null;
        }
    }

    [Header("Board configurations")]
    [SerializeField] private GameObject enemyCardsCombatTypeHintsContainer;
    [SerializeField] private Transform enemyCardsCombatTypeHintsContainerFinalPosition;
    [SerializeField] private GameObject playerHandContainer;
    [SerializeField] private GameObject playerDeck;
    [SerializeField] private GameObject playerOnCombatCardFinalPosition;
    [SerializeField] private GameObject enemyOnCombatCardFinalPosition;
    [SerializeField] private GameObject combatContainer;
    [SerializeField] private CombatTypeHintComponent combatTypeHintPrefab;
    [SerializeField] private CombatCard combatCardPrefab;
    [SerializeField] private Transform playerTieZone;
    [SerializeField] private Transform enemyTieZone;

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
            enemyCardsCombatTypeHintsContainer,
            enemyCardsCombatTypeHintsContainerFinalPosition,
            playerHandContainer,
            playerDeck,
            playerOnCombatCardFinalPosition,
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
}
