using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CombatManager : MonoBehaviour
{
    [Header("Combat field configuration")]
    [SerializeField]
    private Transform EnemySpawnerOrigin;
    [SerializeField]
    private Transform PlayerCardsSpawnerOrigin;
    [SerializeField]
    private float PlayerCardsHorizontalOffset = 3.0f;

    [Header("Debug")]
    [SerializeField]
    private GameObject DebugEnemyCardPrefab;
    [SerializeField]
    private CombatCardTemplate DebugEnemyCombatCardTemplate;

    private PartyManager PartyManager;
    private CombatStates CurrentState;

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
        GameObject EnemyCard = Instantiate(DebugEnemyCardPrefab, EnemySpawnerOrigin.position, Quaternion.identity);
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
                PlayerCardsSpawnerOrigin.position.x + i * (CardWidth + PlayerCardsHorizontalOffset),
                PlayerCardsSpawnerOrigin.position.y
            );

            Instantiate(PartyMembers[i], SpawnPosition, Quaternion.identity);
        }
    }
}
