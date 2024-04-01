using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    private struct PartyMemberInSceneInfo
    {
        public PartyMember partyMemberInScene;
        public GameObject partyMemberGameObject;
        public Vector2 positionInHand;
        public bool isCardActive;

        public PartyMemberInSceneInfo(
            PartyMember partyMemberInScene,
            GameObject partyMemberGameObject,
            Vector2 positionInHand)
        {
            this.partyMemberInScene = partyMemberInScene;
            this.partyMemberGameObject = partyMemberGameObject;
            this.positionInHand = positionInHand;
            this.isCardActive = true;
        }

        public void Reset()
        {
            partyMemberInScene = null;
            partyMemberGameObject = null;
            positionInHand = new Vector2();
            isCardActive = true;
        }
    }

    [Header("Combat field configuration")]
    [Header("Cards positions")]
    [SerializeField] private Transform enemyCardOrigin;
    [SerializeField] private Transform attackerCardOrigin;
    [SerializeField] private Transform caughtCardOrigin;
    [SerializeField] private Transform playerCardsOrigin;
    [SerializeField] private float playerCardsHorizontalOffset = 3.0f;
    
    [Header("Swipe actions texts")]
    [SerializeField] private string placeAsAttackerCardText;
    [SerializeField] private string changeAttackerCardText;
    [SerializeField] private string attackText;
    [SerializeField] private string captureCardText;
    [SerializeField] private string letGoCardText;

    [Header("UI configuration")]
    [SerializeField] private CombatSceneController combatSceneUIController;

    [Header("Debug")]
    [SerializeField] private CombatCardTemplate debugEnemyCombatCardTemplate;

    private PartyManager partyManager;
    private CombatStates currentState;
    private List<PartyMemberInSceneInfo> partyMembersInScene;
    private PartyMemberInSceneInfo currentAttacker;
    private CombatCard enemyCard;
    private int combatTurns;
    private CombatCardTemplate enemyCombatCardTemplate;

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
                InitCombatField();
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
                EndTurnCycle();
                break;
            case CombatStates.COMBAT_WON:
                ShowEnemyAsACaughtablePartyMember();
                break;
            case CombatStates.COMBAT_LOST:
                StartEndCombatSequence(TurnResult.COMBAT_LOST);
                break;
            case CombatStates.GAME_OVER:
                StartEndCombatSequence(TurnResult.COMBAT_GAME_OVER);
                break;
            default:
                break;
        }
    }

    public CombatStates GetCurrentCombatState()
    {
        return currentState;
    }

    void InitCombatField() {
        enemyCard = SpawnEnemyCard();

        combatSceneUIController.SetTurnNumberAsAnimation(enemyCard.GetCombatTurnsForCard(),
            onAnimationEnded: () =>
            {
                UpdateCombatTurns(enemyCard.GetCombatTurnsForCard());
            });

        CombatSceneManager.Instance.ProvideCombatVisualManager().PlayMoveEnemyCardAnimation(
            enemyCardToMove: enemyCard.gameObject,
            onAnimationEnded: () =>
            {
                combatSceneUIController.ShowDialogText(enemyCard.GetInitialText(),
                    onAnimationEnded: () => {
                        SpawnPlayerCards();
                        SetCombatState(CombatStates.CHOOSE_ATTACKER);
                    });
            });
    }

    void UpdateCombatTurns(int newCombatTurns)
    {
        combatTurns = newCombatTurns;
        combatSceneUIController.SetTurnNumber(combatTurns);
    }

    CombatCard SpawnEnemyCard()
    {
        GameObject combatCardPrefab = (GameObject) Resources.Load("Prefabs/EnemyCombatCard");
        GameObject enemyCard = Instantiate(combatCardPrefab, enemyCardOrigin.position, Quaternion.identity);
        CombatCard combatCardComponent = enemyCard.GetComponent<CombatCard>();
        if (combatCardPrefab.GetComponent<CombatCard>() != null)
        {
            enemyCombatCardTemplate = GameManager.Instance.ActualCombatEnemyCard ?
                GameManager.Instance.ActualCombatEnemyCard : debugEnemyCombatCardTemplate;
            combatCardComponent.SetDataCard(enemyCombatCardTemplate);
        }
        
        return combatCardComponent;
    }

    void SpawnPlayerCards()
    {
        List<PartyMember> partyMembers = partyManager.GetPartyMembers();
        GameObject combatCardPrefab = (GameObject) Resources.Load("Prefabs/PlayerCombatCard");

        for (int i = 0; i < partyMembers.Count; i++)
        {
            // Setting up PartyMember as a GameObject (physically) in scene
            GameObject partyMemberGameObject = Instantiate(combatCardPrefab);
            
            float cardWidth = 0.0f;
            CombatCard combatCardComponent = partyMemberGameObject.GetComponent<CombatCard>();
            if (combatCardComponent)
            {
                combatCardComponent.SetDataCard(partyMembers[i].CombatCardTemplate);
                cardWidth = combatCardComponent.GetCardWidth();
            }

            Vector2 spawnPosition = new Vector2(
                playerCardsOrigin.position.x + i * (cardWidth + playerCardsHorizontalOffset),
                playerCardsOrigin.position.y
            );

            partyMemberGameObject.transform.position = spawnPosition;

            // Setting up PartyMember as an object in the local scene logic context
            PartyMemberInSceneInfo partyMemberInScene = new PartyMemberInSceneInfo(partyMembers[i], partyMemberGameObject, spawnPosition);
            SetUpPartyMemberCard(partyMemberInScene);

            partyMembersInScene.Add(partyMemberInScene);
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
                partyMemberCombatCardComponent.SetTopSwipeWarningText(placeAsAttackerCardText);
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
                partyMemberCombatCardComponent.SetLeftSwipeWarningText(changeAttackerCardText);
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
                partyMemberCombatCardComponent.SetRightSwipeWarningText(attackText);
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
        ReturnAttackerCardToHand();

        currentAttacker = NewAttacker;
        currentAttacker.partyMemberGameObject.transform.position = attackerCardOrigin.position;
        currentAttacker.partyMemberGameObject.transform.rotation = attackerCardOrigin.rotation;

        SetCombatState(CombatStates.CHOOSE_ACTION);
    }

    void ReturnAttackerCardToHand()
    {
        if (currentAttacker.partyMemberGameObject)
        {
            currentAttacker.partyMemberGameObject.transform.position = currentAttacker.positionInHand;
            currentAttacker.partyMemberGameObject.transform.rotation = Quaternion.identity;
        }
        currentAttacker.partyMemberGameObject = null;
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
                    if (areCardsActive && partyMemberCombatCardComponent.GetCardCurrentEnergy() > 0)
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

        if (CurrentAttackerCombatCard && enemyCard)
        {
            CombatUtils.Attack(
                attackerCombatCard: CurrentAttackerCombatCard,
                defenderCombatCard: enemyCard,
                out AttackFinalEffectiveness
            );
            GameManager.Instance.ProvideBrainSoundManager().PlaySoundCombat(AttackFinalEffectiveness);
            CurrentAttackerCombatCard.ReduceAttackerEnergy(energyToReduce: 1);
        }

        SetCombatState(CombatStates.ENEMY_ATTACK);
    }

    void PerformEnemyAttackAction()
    {
        AttackEffectiveness attackFinalEffectiveness = AttackEffectiveness.NEUTRAL;
        CombatCard currentAttackerCombatCard = currentAttacker.partyMemberGameObject.GetComponent<CombatCard>();

        if (currentAttackerCombatCard && enemyCard && combatSceneUIController)
        {
            CombatUtils.Attack(
                attackerCombatCard: enemyCard,
                defenderCombatCard: currentAttackerCombatCard,
                out attackFinalEffectiveness
            );

            if (enemyCard.GetHealthPoints() > 0)
            {
                switch (attackFinalEffectiveness)
                {
                    case AttackEffectiveness.NEUTRAL:
                        HideEnemyDialogBubble();
                        break;
                    case AttackEffectiveness.SUPER_EFFECTIVE:
                        combatSceneUIController.ShowDialogText(enemyCard.GetSuperEffectiveText());
                        break;
                    case AttackEffectiveness.NOT_VERY_EFFECTIVE:
                        combatSceneUIController.ShowDialogText(enemyCard.GetNotVeryEffectiveText());
                        break;
                }
            }
        }

        SetCombatState(CombatStates.CHECK_COMBAT_RESULTS);
    }

    void HideEnemyDialogBubble()
    {
        if (combatSceneUIController)
        {
            combatSceneUIController.HideDialogText();
        }
    }

    void EndTurnCycle()
    {
        UpdateCombatTurns(--combatTurns);
        RecoverEnergyForCardsInHand();
        CheckCombatResults();
    }
    
    void RecoverEnergyForCardsInHand()
    {
        foreach (PartyMemberInSceneInfo partyMemberInScene in partyMembersInScene)
        {
            if (partyMemberInScene.partyMemberGameObject != currentAttacker.partyMemberGameObject)
            {
                CombatCard partyMemberCombatCardComponent =
                    partyMemberInScene.partyMemberGameObject.GetComponent<CombatCard>();
                if (partyMemberCombatCardComponent)
                {
                    partyMemberCombatCardComponent.RecoverEnergy(energyToRecover: 1);
                }
            }
        }
    }

    void CheckCombatResults()
    {
        CombatCard CurrentAttackerCombatCard = currentAttacker.partyMemberGameObject.GetComponent<CombatCard>();
        if (CurrentAttackerCombatCard && enemyCard)
        {
            if (enemyCard.GetHealthPoints() <= 0)
            {
                SetCombatState(CombatStates.COMBAT_WON);
            }
            else if (CurrentAttackerCombatCard.GetHealthPoints() > 0 &&
                CurrentAttackerCombatCard.GetCardCurrentEnergy() > 0)
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
            else if (CurrentAttackerCombatCard.GetCardCurrentEnergy() == 0)
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

            if (combatTurns <= 0 
                && currentState != CombatStates.GAME_OVER
                && currentState != CombatStates.COMBAT_WON) 
            {
                SetCombatState(CombatStates.COMBAT_LOST);
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

    // void EndCombat(bool combatWon)
    // {
    //     int i = 0;
    //     foreach(var partyMember in partyMembersInScene)
    //     {

    //         GameUtils.CreateTemporizer(() => { Destroy(partyMember.partyMemberGameObject); }, i, this);
    //         i++;
    //     }
    //     GameUtils.CreateTemporizer(() => { Destroy(enemyCard.gameObject); }, i, this);
    //     i++;
    //     if(combatWon)
    //     {
    //         GameUtils.CreateTemporizer(() => {CaptureEnemy(); }, i, this);
    //     }
    //     else
    //     {
    //         GameUtils.CreateTemporizer(() => { CaptureEnemy(); }, i, this);
    //     }
        
    // }

    // void CaptureEnemy()
    // {
    //     GameObject enemyCard = Instantiate(combatCardPrefab, Vector2.zero, Quaternion.identity);
    //     CombatCard combatCardComponent = enemyCard.GetComponent<CombatCard>();
    //     if (combatCardPrefab.GetComponent<CombatCard>() != null)
    //     {
    //         combatCardComponent.SetDataCard(GameManager.Instance.ActualCombatEnemyCard);
    //     }
    //     InteractiveCombatCardComponent enemyInteractiveCombatCardComponent = enemyCard.GetComponent<InteractiveCombatCardComponent>();
    //     if (enemyInteractiveCombatCardComponent)
    //     {
    //         enemyInteractiveCombatCardComponent.SetOnSwipeLeftAction(() =>
    //         {
    //             GameUtils.CreateTemporizer(() => {GameManager.Instance.EndCombat(TurnResult.COMBAT_WON_NO_CAPTURE); }, 2, this);
    //         });
    //         enemyInteractiveCombatCardComponent.SetOnSwipeRightAction(() =>
    //         {
    //             GameUtils.CreateTemporizer(() => {GameManager.Instance.EndCombat(TurnResult.COMBAT_WON_CAPTURE);
    //             GameManager.Instance.ProvidePartyManager().AddPartyMember(GameManager.Instance.ActualCombatEnemyCard);
    //             }, 2, this);
    //         });
    //         enemyInteractiveCombatCardComponent.EnableHorizontalDraggableComponent();
    //     }
        
    // }

    void ShowEnemyAsACaughtablePartyMember()
    {
        SetUpEnemyCardCaughtableState();
        ReturnAttackerCardToHand();
        UpdatePartyMembersStateAfterBattle();
        SetPartyMembersCardsInHandActivation(false);
    }

    void SetUpEnemyCardCaughtableState()
    {
        void SetUpEnemyOnSwipeLeftActions(
            CombatCard enemyCombatCardComponent, 
            InteractiveCombatCardComponent enemyInteractiveCombarCardComponent)
        {
            enemyCombatCardComponent.SetLeftSwipeWarningText(letGoCardText);

            enemyInteractiveCombarCardComponent.SetOnSwipeLeftAction(() =>
            {
                StartEndCombatSequence(TurnResult.COMBAT_WON_NO_CAPTURE);
            });

            enemyInteractiveCombarCardComponent.SetOnSwipeLeftEscapeZoneActions(
                () => { enemyCombatCardComponent.EnableLeftSwipeWarningText(); },
                () => { enemyCombatCardComponent.DisableWarningText(); }
            );
        }

        void SetUpEnemyOnSwipeRightActions(
            CombatCard enemyCombatCardComponent, 
            InteractiveCombatCardComponent enemyInteractiveCombarCardComponent)
        {
            enemyCombatCardComponent.SetRightSwipeWarningText(captureCardText);

            enemyInteractiveCombarCardComponent.SetOnSwipeRightAction(() =>
            {
                AddOrSwapEnemyAsPartyMember();
            });

            enemyInteractiveCombarCardComponent.SetOnSwipeRightEscapeZoneActions(
                () => { enemyCombatCardComponent.EnableRightSwipeWarningText(); },
                () => { enemyCombatCardComponent.DisableWarningText(); }
            );
        }

        enemyCard.transform.position = caughtCardOrigin.position;
        enemyCard.transform.rotation = caughtCardOrigin.rotation;

        CombatCard enemyCombatCardComponent = enemyCard.GetComponent<CombatCard>();
        InteractiveCombatCardComponent enemyInteractiveCombarCardComponent =
            enemyCard.GetComponent<InteractiveCombatCardComponent>();
        if (enemyCombatCardComponent && enemyInteractiveCombarCardComponent)
        {
            SetUpEnemyOnSwipeLeftActions(enemyCombatCardComponent, enemyInteractiveCombarCardComponent);
            SetUpEnemyOnSwipeRightActions(enemyCombatCardComponent, enemyInteractiveCombarCardComponent);
            enemyInteractiveCombarCardComponent.EnableHorizontalDraggableComponent();
        }
    }

    void AddOrSwapEnemyAsPartyMember()
    {
        // A party member must be removed
        if (partyManager.GetPartyCount() == partyManager.GetMaxPartySize())
        {
            SetUpPartyMemberCardsForSwapingWithEnemy();
        }
        else
        {
            partyManager.AddPartyMember(enemyCombatCardTemplate);
            StartEndCombatSequence(TurnResult.COMBAT_WON_CAPTURE);
        }
    }

    void SetUpPartyMemberCardsForSwapingWithEnemy()
    {
        void SetUpOnSwipeUpActions(PartyMemberInSceneInfo partyMemberInScene)
        {
            InteractiveCombatCardComponent partyMemberInteractiveCombatCardComponent =
                partyMemberInScene.partyMemberGameObject.GetComponent<InteractiveCombatCardComponent>();
            if (partyMemberInteractiveCombatCardComponent)
            {
                partyMemberInteractiveCombatCardComponent.SetOnSwipeUpAction(() =>
                {
                    InteractiveCombatCardComponent enemyInteractiveCombatCardComponent =
                        enemyCard.GetComponent<InteractiveCombatCardComponent>();
                    if (enemyInteractiveCombatCardComponent)
                    {
                        enemyInteractiveCombatCardComponent.DisableDraggableComponents();

                        partyMembersInScene.Remove(partyMemberInScene);
                        partyManager.RemovePartyMember(partyMemberInScene.partyMemberInScene.CombatCardTemplate);

                        Destroy(partyMemberInScene.partyMemberGameObject);
                        partyManager.AddPartyMember(enemyCombatCardTemplate);

                        StartEndCombatSequence(TurnResult.COMBAT_WON_CAPTURE);
                    }
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
                partyMemberCombatCardComponent.SetTopSwipeWarningText(letGoCardText);
                partyMemberInteractiveCombatCardComponent.SetOnSwipeUpEscapeZoneActions(
                    () => { partyMemberCombatCardComponent.EnableTopSwipeWarningText(); },
                    () => { partyMemberCombatCardComponent.DisableWarningText(); }
                );
            }
        }

        foreach (PartyMemberInSceneInfo partyMemberInScene in partyMembersInScene)
        {
            SetUpOnSwipeUpActions(partyMemberInScene);
            SetUpOnSwipeUpWarningActions(partyMemberInScene);
            CombatCard partyMemberCombatCardComponent =
                    partyMemberInScene.partyMemberGameObject.GetComponent<CombatCard>();
            InteractiveCombatCardComponent partyMemberInteractiveCombatCardComponent =
                partyMemberInScene.partyMemberGameObject.GetComponent<InteractiveCombatCardComponent>();
            if (partyMemberCombatCardComponent && partyMemberInteractiveCombatCardComponent)
            {
                partyMemberCombatCardComponent.SetInactiveOverlayActivation(false);
                partyMemberInteractiveCombatCardComponent.EnableVerticalDraggableComponent();
            }
        }
    }

    void UpdatePartyMembersStateAfterBattle()
    {
        partyManager.ClearParty();
        foreach (PartyMemberInSceneInfo partyMemberInScene in partyMembersInScene)
        {
            CombatCard partyMemberCombatCardComponent = 
                partyMemberInScene.partyMemberGameObject.GetComponent<CombatCard>();
            if (partyMemberCombatCardComponent)
            {
                partyManager.AddPartyMember(
                    cardToAdd: partyMemberInScene.partyMemberInScene.CombatCardTemplate,
                    cardHealthPoints: partyMemberCombatCardComponent.GetHealthPoints(),
                    cardEnergyPoints: partyMemberCombatCardComponent.GetCardCurrentEnergy());
            }
        }
    }

    // void InitGameOverSequence()
    // {
    //     GameObject enemyCard = Instantiate(combatCardPrefab, Vector2.zero, Quaternion.identity);
    //     CombatCard combatCardComponent = enemyCard.GetComponent<CombatCard>();
    //     if (combatCardPrefab.GetComponent<CombatCard>() != null)
    //     {
    //         combatCardComponent.SetDataCard(GameManager.Instance.ActualCombatEnemyCard);
    //     }
    //     HorizontalDraggableComponent draggableComponent = enemyCard.GetComponent<HorizontalDraggableComponent>();
    //     GameUtils.CreateTemporizer(() => {GameManager.Instance.EndCombat(TurnResult.COMBAT_LOST); }, 2, this);

    // }

    void StartEndCombatSequence(TurnResult combatResult)
    {
        GameManager.Instance.ProvideBrainSoundManager()
            .EndCombat(GameManager.Instance.ProvideBrainManager().bIsBossFight);
        GameUtils.CreateTemporizer(() => 
        {
            GameManager.Instance.EndCombat(combatResult);
        }, 1.0f, this);
    }
}
