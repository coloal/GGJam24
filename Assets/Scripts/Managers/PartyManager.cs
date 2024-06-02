using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance;

    [Header("Party setup")]
    [SerializeField] private List<CombatCardTemplate> initialPartyMembersCombatCardTemplates;
    [SerializeField] private int maxPartySize = 5;

    private List<CombatCardTemplate> partyMembers;

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
        partyMembers = new List<CombatCardTemplate>();
        foreach (CombatCardTemplate InitialPartyMember in initialPartyMembersCombatCardTemplates)
        {
            partyMembers.Add(InitialPartyMember);
        }
    }

    public void AddPartyMember(CombatCardTemplate cardToAdd)
    {
        partyMembers.Add(cardToAdd);
    }

    public int GetPartyCount()
    {
        return partyMembers.Count;
    }

    public bool CheckPartyMember(CombatCardTemplate card)
    {
        return partyMembers.Find(x=>x.Equals(card)) != null;
    }

    void Start()
    {

    }

    public List<CombatCardTemplate> GetPartyMembers()
    {
        return partyMembers;
    }

    public void RemovePartyMember(CombatCardTemplate cardToRemove)
    {
        partyMembers.RemoveAll(Card => { return Card.Equals(cardToRemove); });
    }

    public int GetMaxPartySize()
    {
        return maxPartySize;
    }

    public void ClearParty()
    {
        partyMembers.Clear();
    }
}
