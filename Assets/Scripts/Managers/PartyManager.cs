using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [Header("Party setup")]
    [SerializeField]
    private List<CombatCardTemplate> InitialPartyMembersCombatCardTemplates;

    public static PartyManager Instance;
    private List<PartyMember> PartyMembers;

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
        PartyMembers = new List<PartyMember>();
        foreach (CombatCardTemplate InitialPartyMember in InitialPartyMembersCombatCardTemplates)
        {
            PartyMembers.Add(new PartyMember(InitialPartyMember));
        }
    }


    public void AddMemberToParty(CombatCardTemplate card)
    {
        PartyMembers.Add(new PartyMember(card));
    }

    public int GetPartyCount()
    {
        return PartyMembers.Count;
    }

    public bool CheckPartyMember(CombatCardTemplate card)
    {
        return PartyMembers.Find(x=>x.CombatCardTemplate.Equals(card)) != null;
    }

    void Start()
    {

    }

    public List<PartyMember> GetPartyMembers()
    {
        return PartyMembers;
    }
}
