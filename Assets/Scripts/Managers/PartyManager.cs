using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [Header("Party setup")]
    [SerializeField]
    private List<CombatCardTemplate> InitialPartyMembersCombatCardTemplates;
    private CombatCard CombatCardPrefab;

    public static PartyManager Instance;
    private List<GameObject> PartyMembers;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        Init();
    }

    void Init()
    {
        PartyMembers = new List<GameObject>();
        foreach (CombatCardTemplate InitialPartyMember in InitialPartyMembersCombatCardTemplates)
        {
            GameObject PartyMember = (GameObject) Resources.Load("Prefabs/CombatCard");
            CombatCard PartyMemberCombatCardComponent = PartyMember.GetComponent<CombatCard>();
            if (PartyMemberCombatCardComponent)
            {
                PartyMemberCombatCardComponent.SetDataCard(InitialPartyMember);
            }

            PartyMembers.Add(PartyMember);
        }
    }

    void Start()
    {

    }

    public List<GameObject> GetPartyMembers()
    {
        return PartyMembers;
    }
}
