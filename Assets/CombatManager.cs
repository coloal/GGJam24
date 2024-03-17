using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CombatManager : MonoBehaviour
{
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

    private PartyManager PartyManager;
    private CombatStates CurrentState;
    private List<GameObject> PartyMembersInScene;

    void Awake()
    {
        PartyMembersInScene = new List<GameObject>();
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
        GameObject EnemyCard = Instantiate(DebugEnemyCardPrefab, EnemyCardOrigin.position, Quaternion.identity);
        CombatCard EnemyCardCombatCardComponent = EnemyCard.GetComponent<CombatCard>();
        if (EnemyCardCombatCardComponent)
        {
            EnemyCardCombatCardComponent.SetDataCard(DebugEnemyCombatCardTemplate);
        }
    }

    void SpawnPlayerCards()
    {
        List<GameObject> PartyMembers = PartyManager.GetPartyMembers();
        for (int i = 0; i < PartyMembers.Count; i++)
        {
            float CardWidth = 0.0f;
            CombatCard CombatCardComponent = PartyMembers[i].GetComponent<CombatCard>();
            if (CombatCardComponent)
            {
                CardWidth = CombatCardComponent.GetCardWidth();
            }

            Vector2 SpawnPosition = new Vector2(
                PlayerCardsOrigin.position.x + i * (CardWidth + PlayerCardsHorizontalOffset),
                PlayerCardsOrigin.position.y
            );

            GameObject PartyMemberInScene = Instantiate(PartyMembers[i], SpawnPosition, Quaternion.identity);
            PartyMembersInScene.Add(PartyMemberInScene);
        }
    }

    void ChooseAttacker()
    {
        SetPartyMembersCardsActivation(true);
        foreach (GameObject PartyMember in PartyMembersInScene)
        {
            InteractiveCombatCardComponent PartyMemberInteractiveCombatCardComponent =
                PartyMember.GetComponent<InteractiveCombatCardComponent>();
            if (PartyMemberInteractiveCombatCardComponent)
            {
                PartyMemberInteractiveCombatCardComponent.SetOnClickAction(() => {
                    PartyMember.transform.position = AttackerCardOrigin.position;
                });
            }
        }
    }

    void SetPartyMembersCardsActivation(bool AreCardsActive)
    {
        foreach (GameObject PartyMember in PartyMembersInScene)
        {
            InteractiveCombatCardComponent PartyMemberInteractiveCombatCardComponent =
                PartyMember.GetComponent<InteractiveCombatCardComponent>();
            if (PartyMemberInteractiveCombatCardComponent)
            {
                PartyMemberInteractiveCombatCardComponent.SetIsActive(AreCardsActive);
            }
        }
    }
}
