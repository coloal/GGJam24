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
        public EnemyTemplate enemyTemplate;
        public GameObject combatContainer;

        public CombatContext(GameObject enemyCardsCombatTypeHintsContainer,
            Transform enemyCardsCombatTypeHintsContainerFinalPosition,
            GameObject playerHandContainer,
            GameObject playerDeck,
            GameObject playerOnCombatCardFinalPosition,
            GameObject enemyOnCombatCardFinalPosition,
            EnemyTemplate enemyTemplate,
            GameObject combatContainer)
        {
            this.enemyCardsCombatTypeHintsContainer = enemyCardsCombatTypeHintsContainer;
            this.enemyCardsCombatTypeHintsContainerFinalPosition = enemyCardsCombatTypeHintsContainerFinalPosition;
            this.playerHandContainer = playerHandContainer;
            this.playerDeck = playerDeck;
            this.playerOnCombatCardFinalPosition = playerOnCombatCardFinalPosition;
            this.enemyOnCombatCardFinalPosition = enemyOnCombatCardFinalPosition;
            this.enemyTemplate = enemyTemplate;
            this.combatContainer = combatContainer;

            this.playerOnCombatCard = null;
            this.enemyOnCombatCard = null;
        }
    }

    [Header("Board configurations")]
    [SerializeField] private GameObject EnemyCardsCombatTypeHintsContainer;
    [SerializeField] private Transform EnemyCardsCombatTypeHintsContainerFinalPosition;
    [SerializeField] private GameObject PlayerHandContainer;
    [SerializeField] private GameObject PlayerDeck;
    [SerializeField] private GameObject PlayerOnCombatCardFinalPosition;
    [SerializeField] private GameObject EnemyOnCombatCardFinalPosition;
    [SerializeField] private EnemyTemplate EnemyTemplate;
    [SerializeField] private GameObject CombatContainer;
    [SerializeField] private CombatTypeHintComponent combatTypeHintPrefab;

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
            EnemyCardsCombatTypeHintsContainer,
            EnemyCardsCombatTypeHintsContainerFinalPosition,
            PlayerHandContainer,
            PlayerDeck,
            PlayerOnCombatCardFinalPosition,
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

    public void OverwriteCombatContext(CombatContext newCombatContext)
    {
        combatContext = newCombatContext;
    }

    public GameObject InstantiateCombatTypeHintGameObject()
    {
        return Instantiate(combatTypeHintPrefab).gameObject;
    }
}
