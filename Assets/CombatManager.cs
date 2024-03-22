using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CombatManager : MonoBehaviour
{
    private struct PartyMemberInSceneInfo
    {
        public GameObject PartyMember;
        public Vector2 PositionInScene;

        public PartyMemberInSceneInfo(GameObject PartyMember, Vector2 PositionInScene)
        {
            this.PartyMember = PartyMember;
            this.PositionInScene = PositionInScene;
        }
    }

    [Header("Combat field configuration")]
    [Header("Cards positions")]
    [SerializeField]
    private Transform EnemyCardOrigin;
    [SerializeField]
    private Transform PlayerCardsOrigin;
    [SerializeField]
    private float PlayerCardsHorizontalOffset = 3.0f;
    [SerializeField]
    private Transform AttackerCardOrigin;


    [Header("Debug")]
    [SerializeField]
    private GameObject DebugEnemyCardPrefab;
    [SerializeField]
    private CombatCardTemplate DebugEnemyCombatCardTemplate;
    [SerializeField]
    private GameObject DebugActionButtons;

    private PartyManager PartyManager;
    private CombatStates CurrentState;
    private List<PartyMemberInSceneInfo> PartyMembersInScene;
    private PartyMemberInSceneInfo CurrentAttacker;
    private GameObject EnemyCard;

    void Awake()
    {
        PartyMembersInScene = new List<PartyMemberInSceneInfo>();
        CurrentAttacker.PartyMember = null;
    }

    void Start()
    {
        SetUpManagers();
        SetCombatState(CombatStates.INIT);
    }

    void SetUpManagers()
    {
        PartyManager = GameManager.Instance.ProvidePartyManager();
    }

    public void SetCombatState(CombatStates State)
    {
        Debug.Log("Started: " + State.ToString());
        CurrentState = State;
        ProcessCurrentCombatState();
    }

    void ProcessCurrentCombatState()
    {
        switch (CurrentState)
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
            // case CombatStates.PLAYER_ATTACK:
            //     PerformPlayerAttackAction();
            //     break;
            case CombatStates.ENEMY_ATTACK:
                PerformEnemyAttackAction();
                break;
            default:
                break;
        }
    }

    public CombatStates GetCurrentCombatState()
    {
        return CurrentState;
    }

    void SpawnCombatCards() {
        SpawnEnemyCard();
        SpawnPlayerCards();

        SetCombatState(CombatStates.CHOOSE_ATTACKER);
    }

    void SpawnEnemyCard()
    {
        EnemyCard = Instantiate(DebugEnemyCardPrefab, EnemyCardOrigin.position, Quaternion.identity);
        CombatCard EnemyCardCombatCardComponent = EnemyCard.GetComponent<CombatCard>();
        if (EnemyCardCombatCardComponent)
        {
            EnemyCardCombatCardComponent.SetDataCard(DebugEnemyCombatCardTemplate);
        }
    }

    void SpawnPlayerCards()
    {
        List<PartyMember> PartyMembers = PartyManager.GetPartyMembers();
        GameObject CombatCardPrefab = (GameObject) Resources.Load("Prefabs/CombatCard");

        for (int i = 0; i < PartyMembers.Count; i++)
        {
            GameObject PartyMemberInScene = Instantiate(CombatCardPrefab);
            
            float CardWidth = 0.0f;
            CombatCard CombatCardComponent = PartyMemberInScene.GetComponent<CombatCard>();
            if (CombatCardComponent)
            {
                CombatCardComponent.SetDataCard(PartyMembers[i].CombatCardTemplate);
                CardWidth = CombatCardComponent.GetCardWidth();
            }

            Vector2 SpawnPosition = new Vector2(
                PlayerCardsOrigin.position.x + i * (CardWidth + PlayerCardsHorizontalOffset),
                PlayerCardsOrigin.position.y
            );

            PartyMemberInScene.transform.position = SpawnPosition;

            PartyMembersInScene.Add(new PartyMemberInSceneInfo(PartyMemberInScene, SpawnPosition));
        }
    }

    void ChooseAttacker()
    {
        DebugActionButtons.SetActive(false);

        SetPartyMembersCardsActivation(true);
        foreach (PartyMemberInSceneInfo PartyMemberInScene in PartyMembersInScene)
        {
            InteractiveCombatCardComponent PartyMemberInteractiveCombatCardComponent =
                PartyMemberInScene.PartyMember.GetComponent<InteractiveCombatCardComponent>();
            if (PartyMemberInteractiveCombatCardComponent)
            {
                PartyMemberInteractiveCombatCardComponent.SetOnClickAction(() => {
                    SetCurrentAttacker(PartyMemberInScene);
                });
            }
        }
    }

    void SetCurrentAttacker(PartyMemberInSceneInfo NewAttacker)
    {
        // If there's already an atacker card, swap it with the new card
        if (CurrentAttacker.PartyMember)
        {
            CurrentAttacker.PartyMember.transform.position = CurrentAttacker.PositionInScene;
        }

        CurrentAttacker = NewAttacker;
        CurrentAttacker.PartyMember.transform.position = AttackerCardOrigin.position;

        SetCombatState(CombatStates.CHOOSE_ACTION);
    }

    void SetPartyMembersCardsActivation(bool AreCardsActive)
    {
        foreach (PartyMemberInSceneInfo PartyMemberInScene in PartyMembersInScene)
        {
            InteractiveCombatCardComponent PartyMemberInteractiveCombatCardComponent =
                PartyMemberInScene.PartyMember.GetComponent<InteractiveCombatCardComponent>();
            if (PartyMemberInteractiveCombatCardComponent)
            {
                PartyMemberInteractiveCombatCardComponent.SetIsActive(AreCardsActive);
            }
        }
    }

    void ChooseAttackerAction()
    {
        // DEBUG PURPOSES ONLY
        DebugActionButtons.SetActive(true);

        SetPartyMembersCardsActivation(false);
        if (CurrentAttacker.PartyMember)
        {
            //TODO: Make the attacker card a *Draggable* card to make decisions
            //CurrentAttacker.AddComponent<Draggable>();
        }
    }

    // DEBUG PURPOSES ONLY
    public void PerformPlayerAttackAction()
    {
        // DEBUG PURPOSES ONLY
        SetCombatState(CombatStates.PLAYER_ATTACK);

        AttackEffectiveness AttackFinalEffectiveness = AttackEffectiveness.NEUTRAL;
        CombatCard CurrentAttackerCombatCard = CurrentAttacker.PartyMember.GetComponent<CombatCard>();
        CombatCard EnemyCombatCard = EnemyCard.GetComponent<CombatCard>();

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
        DebugActionButtons.SetActive(false);

        AttackEffectiveness AttackFinalEffectiveness = AttackEffectiveness.NEUTRAL;
        CombatCard CurrentAttackerCombatCard = CurrentAttacker.PartyMember.GetComponent<CombatCard>();
        CombatCard EnemyCombatCard = EnemyCard.GetComponent<CombatCard>();

        if (CurrentAttackerCombatCard && EnemyCard)
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
}
