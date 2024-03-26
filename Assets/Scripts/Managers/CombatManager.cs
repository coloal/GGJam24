using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Rendering;

public class CombatManager : MonoBehaviour
{
    private struct PartyMemberInSceneInfo
    {
        public PartyMember partyMemberInScene;
        public GameObject partyMemberGameObject;
        public Vector2 positionInHand;

        public PartyMemberInSceneInfo(
            PartyMember partyMemberInScene,
            GameObject partyMemberGameObject,
            Vector2 positionInHand)
        {
            this.partyMemberInScene = partyMemberInScene;
            this.partyMemberGameObject = partyMemberGameObject;
            this.positionInHand = positionInHand;
        }

        public void Reset()
        {
            partyMemberInScene = null;
            partyMemberGameObject = null;
            positionInHand = new Vector2();
        }
    }

    [Header("Combat field configuration")]
    [Header("Cards positions")]
    [SerializeField]
    private Transform enemyCardOrigin;
    [SerializeField]
    private Transform playerCardsOrigin;
    [SerializeField]
    private float playerCardsHorizontalOffset = 3.0f;
    [SerializeField]
    private Transform attackerCardOrigin;


    [Header("Debug")]
    [SerializeField]
    private GameObject debugEnemyCardPrefab;
    [SerializeField]
    private CombatCardTemplate debugEnemyCombatCardTemplate;

    private PartyManager partyManager;
    private CombatStates currentState;
    private List<PartyMemberInSceneInfo> partyMembersInScene;
    private PartyMemberInSceneInfo currentAttacker;
    private GameObject enemyCard;

    void Awake()
    {
        partyMembersInScene = new List<PartyMemberInSceneInfo>();
        currentAttacker.Reset();
    }

    void Start()
    {
        SetUpManagers();
        SetCombatState(CombatStates.INIT);
    }

    void SetUpManagers()
    {
        partyManager = GameManager.Instance.ProvidePartyManager();
    }

    public void SetCombatState(CombatStates state)
    {
        Debug.Log("Started: " + state.ToString());
        currentState = state;
        ProcessCurrentCombatState();
    }

    void ProcessCurrentCombatState()
    {
        switch (currentState)
        {
            case CombatStates.INIT:
                SpawnCombatCards();
                break;
            case CombatStates.CHOOSE_ATTACKER:
                ChooseAttacker();
                break;
            case CombatStates.CHOOSE_ACTION:
                ChooseAttackerAction();
                break;
            case CombatStates.PLAYER_ATTACK:
                PerformPlayerAttackAction();
                break;
            case CombatStates.ENEMY_ATTACK:
                PerformEnemyAttackAction();
                break;
            case CombatStates.CHECK_COMBAT_RESULTS:
                CheckCombatResults();
                break;
            default:
                break;
        }
    }

    public CombatStates GetCurrentCombatState()
    {
        return currentState;
    }

    void SpawnCombatCards() {
        SpawnEnemyCard();
        SpawnPlayerCards();

        SetCombatState(CombatStates.CHOOSE_ATTACKER);
    }

    void SpawnEnemyCard()
    {
        enemyCard = Instantiate(debugEnemyCardPrefab, enemyCardOrigin.position, Quaternion.identity);
        CombatCard enemyCardCombatCardComponent = enemyCard.GetComponent<CombatCard>();
        if (enemyCardCombatCardComponent)
        {
            enemyCardCombatCardComponent.SetDataCard(debugEnemyCombatCardTemplate);
        }
    }

    void SpawnPlayerCards()
    {
        List<PartyMember> PartyMembers = partyManager.GetPartyMembers();
        GameObject CombatCardPrefab = (GameObject) Resources.Load("Prefabs/CombatCard");

        for (int i = 0; i < PartyMembers.Count; i++)
        {
            // Setting up PartyMember as a GameObject (physically) in scene
            GameObject PartyMemberGameObject = Instantiate(CombatCardPrefab);
            
            float CardWidth = 0.0f;
            CombatCard CombatCardComponent = PartyMemberGameObject.GetComponent<CombatCard>();
            if (CombatCardComponent)
            {
                CombatCardComponent.SetDataCard(PartyMembers[i].CombatCardTemplate);
                CardWidth = CombatCardComponent.GetCardWidth();
            }

            Vector2 SpawnPosition = new Vector2(
                playerCardsOrigin.position.x + i * (CardWidth + playerCardsHorizontalOffset),
                playerCardsOrigin.position.y
            );

            PartyMemberGameObject.transform.position = SpawnPosition;

            // Setting up PartyMember as an object in the local scene logic context
            PartyMemberInSceneInfo PartyMemberInScene = new PartyMemberInSceneInfo(PartyMembers[i], PartyMemberGameObject, SpawnPosition);
            SetUpPartyMemberCard(PartyMemberInScene);

            partyMembersInScene.Add(PartyMemberInScene);
        }
    }

    void SetUpPartyMemberCard(PartyMemberInSceneInfo partyMemberInSceneInfo)
    {
        void SetUpOnSwipeUpActions(PartyMemberInSceneInfo partyMemberInScene)
        {
            InteractiveCombatCardComponent partyMemberInteractiveCombatCardComponent =
                partyMemberInScene.partyMemberGameObject.GetComponent<InteractiveCombatCardComponent>();
            if (partyMemberInteractiveCombatCardComponent)
            {
                partyMemberInteractiveCombatCardComponent.SetOnSwipeUpAction(() =>
                {
                    SetCurrentAttacker(partyMemberInScene);
                });
            }
        }

        void SetUpOnSwipeUpWarningActions(PartyMemberInSceneInfo partyMemberInScene)
        {
            InteractiveCombatCardComponent partyMemberInteractiveCombatCardComponent =
                partyMemberInScene.partyMemberGameObject.GetComponent<InteractiveCombatCardComponent>();
            CombatCard partyMemberCombatCardComponent = 
                partyMemberInScene.partyMemberGameObject.GetComponent<CombatCard>();
            if (partyMemberInteractiveCombatCardComponent && partyMemberCombatCardComponent)
            {
                partyMemberInteractiveCombatCardComponent.SetOnSwipeUpEscapeZoneActions(
                    () => { partyMemberCombatCardComponent.EnableTopSwipeWarningText(); },
                    () => { partyMemberCombatCardComponent.DisableWarningText(); }
                );
            }
        }

        void SetUpOnSwipeLeftActions(PartyMemberInSceneInfo partyMemberInScene)
        {
            InteractiveCombatCardComponent partyMemberInteractiveCombatCardComponent =
                partyMemberInScene.partyMemberGameObject.GetComponent<InteractiveCombatCardComponent>();
            if (partyMemberInteractiveCombatCardComponent)
            {
                partyMemberInteractiveCombatCardComponent.SetOnSwipeLeftAction(() => 
                {
                    SetCombatState(CombatStates.CHOOSE_ATTACKER);
                });
            }
        }

        void SetUpOnSwipeLeftWarningActions(PartyMemberInSceneInfo partyMemberInScene)
        {
            InteractiveCombatCardComponent partyMemberInteractiveCombatCardComponent =
                partyMemberInScene.partyMemberGameObject.GetComponent<InteractiveCombatCardComponent>();
            CombatCard partyMemberCombatCardComponent = 
                partyMemberInScene.partyMemberGameObject.GetComponent<CombatCard>();
            if (partyMemberInteractiveCombatCardComponent && partyMemberCombatCardComponent)
            {
                partyMemberInteractiveCombatCardComponent.SetOnSwipeLeftEscapeZoneActions(
                    () => { partyMemberCombatCardComponent.EnableLeftSwipeWarningText(); },
                    () => { partyMemberCombatCardComponent.DisableWarningText(); }
                );
            }
        }

        void SetUpOnSwipeRightActions(PartyMemberInSceneInfo partyMemberInScene)
        {
            InteractiveCombatCardComponent partyMemberInteractiveCombatCardComponent =
                partyMemberInScene.partyMemberGameObject.GetComponent<InteractiveCombatCardComponent>();
            if (partyMemberInteractiveCombatCardComponent)
            {
                partyMemberInteractiveCombatCardComponent.SetOnSwipeRightAction(() => 
                {
                    SetCombatState(CombatStates.PLAYER_ATTACK);
                });
            }
        }

        void SetUpOnSwipeRightWarningActions(PartyMemberInSceneInfo partyMemberInScene)
        {
            InteractiveCombatCardComponent partyMemberInteractiveCombatCardComponent =
                partyMemberInScene.partyMemberGameObject.GetComponent<InteractiveCombatCardComponent>();
            CombatCard partyMemberCombatCardComponent = 
                partyMemberInScene.partyMemberGameObject.GetComponent<CombatCard>();
            if (partyMemberInteractiveCombatCardComponent && partyMemberCombatCardComponent)
            {
                partyMemberInteractiveCombatCardComponent.SetOnSwipeRightEscapeZoneActions(
                    () => { partyMemberCombatCardComponent.EnableRightSwipeWarningText(); },
                    () => { partyMemberCombatCardComponent.DisableWarningText(); }
                );
            }
        }

        SetUpOnSwipeUpActions(partyMemberInSceneInfo);
        SetUpOnSwipeUpWarningActions(partyMemberInSceneInfo);

        SetUpOnSwipeLeftActions(partyMemberInSceneInfo);
        SetUpOnSwipeLeftWarningActions(partyMemberInSceneInfo);

        SetUpOnSwipeRightActions(partyMemberInSceneInfo);
        SetUpOnSwipeRightWarningActions(partyMemberInSceneInfo);
    }

    void ChooseAttacker()
    {
        SetPartyMembersCardsInHandActivation(true);
        DeactivateAttackerCard();
    }

    void DeactivateAttackerCard()
    {
        if (currentAttacker.partyMemberGameObject)
        {
            CombatCard attackerCombatCardComponent =
            currentAttacker.partyMemberGameObject.GetComponent<CombatCard>();
            InteractiveCombatCardComponent attackerInteractiveCombatCardComponent =
                currentAttacker.partyMemberGameObject.GetComponent<InteractiveCombatCardComponent>();
            if (attackerCombatCardComponent && attackerInteractiveCombatCardComponent)
            {
                attackerCombatCardComponent.SetInactiveOverlayActivation(true);
                attackerInteractiveCombatCardComponent.DisableDraggableComponents();
            }

            currentAttacker.partyMemberGameObject.transform.position = attackerCardOrigin.position;
            currentAttacker.partyMemberGameObject.transform.rotation = attackerCardOrigin.rotation;
        }
    }

    void SetCurrentAttacker(PartyMemberInSceneInfo NewAttacker)
    {
        // If there's already an atacker card, swap it with the new card
        if (currentAttacker.partyMemberGameObject)
        {
            currentAttacker.partyMemberGameObject.transform.position = currentAttacker.positionInHand;
        }

        currentAttacker = NewAttacker;
        currentAttacker.partyMemberGameObject.transform.position = attackerCardOrigin.position;
        currentAttacker.partyMemberGameObject.transform.rotation = attackerCardOrigin.rotation;

        SetCombatState(CombatStates.CHOOSE_ACTION);
    }

    void SetPartyMembersCardsInHandActivation(bool areCardsActive)
    {
        foreach (PartyMemberInSceneInfo partyMemberInScene in partyMembersInScene)
        {
            if (partyMemberInScene.partyMemberGameObject != currentAttacker.partyMemberGameObject)
            {
                CombatCard partyMemberCombatCardComponent =
                partyMemberInScene.partyMemberGameObject.GetComponent<CombatCard>();
                InteractiveCombatCardComponent partyMemberInteractiveCombatCardComponent =
                    partyMemberInScene.partyMemberGameObject.GetComponent<InteractiveCombatCardComponent>();
                if (partyMemberCombatCardComponent && partyMemberInteractiveCombatCardComponent)
                {
                    if (areCardsActive)
                    {
                        partyMemberCombatCardComponent.SetInactiveOverlayActivation(false);
                        partyMemberInteractiveCombatCardComponent.EnableVerticalDraggableComponent();
                    }
                    else
                    {
                        partyMemberCombatCardComponent.SetInactiveOverlayActivation(true);
                        partyMemberInteractiveCombatCardComponent.DisableDraggableComponents();
                    }
                }
            }
        }
    }

    void ChooseAttackerAction()
    {
        SetPartyMembersCardsInHandActivation(false);
        if (currentAttacker.partyMemberGameObject)
        {
            InteractiveCombatCardComponent CurrentAttackerInteractiveCombatCardComponent =
                currentAttacker.partyMemberGameObject.GetComponent<InteractiveCombatCardComponent>();
            if (CurrentAttackerInteractiveCombatCardComponent)
            {
                CurrentAttackerInteractiveCombatCardComponent.EnableHorizontalDraggableComponent();
            }
        }
    }

    public void PerformPlayerAttackAction()
    {
        AttackEffectiveness AttackFinalEffectiveness = AttackEffectiveness.NEUTRAL;
        CombatCard CurrentAttackerCombatCard = currentAttacker.partyMemberGameObject.GetComponent<CombatCard>();
        CombatCard EnemyCombatCard = enemyCard.GetComponent<CombatCard>();

        if (CurrentAttackerCombatCard && EnemyCombatCard)
        {
            CombatUtils.Attack(
                AttackerCombatCard: CurrentAttackerCombatCard,
                DefenderCombatCard: EnemyCombatCard,
                out AttackFinalEffectiveness
            );
            CurrentAttackerCombatCard.ReduceAttackerEnergy();
        }

        SetCombatState(CombatStates.ENEMY_ATTACK);
    }

    void PerformEnemyAttackAction()
    {
        AttackEffectiveness AttackFinalEffectiveness = AttackEffectiveness.NEUTRAL;
        CombatCard CurrentAttackerCombatCard = currentAttacker.partyMemberGameObject.GetComponent<CombatCard>();
        CombatCard EnemyCombatCard = enemyCard.GetComponent<CombatCard>();

        if (CurrentAttackerCombatCard && enemyCard)
        {
            CombatUtils.Attack(
                AttackerCombatCard: EnemyCombatCard,
                DefenderCombatCard: CurrentAttackerCombatCard,
                out AttackFinalEffectiveness
            );
            //TODO: Reduce turns by 1
        }

        SetCombatState(CombatStates.CHECK_COMBAT_RESULTS);
    }

    void CheckCombatResults()
    {
        CombatCard CurrentAttackerCombatCard = currentAttacker.partyMemberGameObject.GetComponent<CombatCard>();
        CombatCard EnemyCombatCard = enemyCard.GetComponent<CombatCard>();
        if (CurrentAttackerCombatCard && EnemyCombatCard)
        {
            if (EnemyCombatCard.GetHealthPoints() <= 0)
            {
                SetCombatState(CombatStates.CAPTURE_ENEMY);
            }
            else if (CurrentAttackerCombatCard.GetHealthPoints() > 0 &&
                CurrentAttackerCombatCard.GetCardEnergy() > 0)
            {
                SetCombatState(CombatStates.CHOOSE_ACTION);
            }
            else if (CurrentAttackerCombatCard.GetHealthPoints() == 0)
            {
                KillCurrentAttackerCard();

                if (partyMembersInScene.Count <= 0)
                {
                    SetCombatState(CombatStates.GAME_OVER);
                }
                else
                {
                    SetCombatState(CombatStates.CHOOSE_ATTACKER);
                }
            }
            else if (CurrentAttackerCombatCard.GetCardEnergy() == 0)
            {
                // 1 represents the out-of-energy card, i.e. there are no cards left in the players' hand
                if (partyMembersInScene.Count <= 1)
                {
                    SetCombatState(CombatStates.GAME_OVER);
                }
                else
                {
                    SetCombatState(CombatStates.CHOOSE_ATTACKER);
                }
            }
        }
    }

    void KillCurrentAttackerCard()
    {
        partyMembersInScene.Remove(currentAttacker);
        partyManager.RemovePartyMember(currentAttacker.partyMemberInScene.CombatCardTemplate);
        Destroy(currentAttacker.partyMemberGameObject);
        currentAttacker.Reset();
    }
}
