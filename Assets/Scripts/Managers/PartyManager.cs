using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance;

    [Header("Party setup")]
    [SerializeField] private List<CombatCardTemplate> initialPartyMembersCombatCardTemplates;
    [SerializeField] private int maxPartySize = 5;

    private List<PartyMember> partyMembers;

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
        partyMembers = new List<PartyMember>();
        foreach (CombatCardTemplate InitialPartyMember in initialPartyMembersCombatCardTemplates)
        {
            partyMembers.Add(new PartyMember(InitialPartyMember));
        }
    }

    public void AddPartyMember(CombatCardTemplate cardToAdd)
    {
        partyMembers.Add(new PartyMember(cardToAdd));
    }

    public void AddPartyMember(CombatCardTemplate cardToAdd, int cardHealthPoints, int cardEnergyPoints)
    {
        partyMembers.Add(new PartyMember(cardToAdd, cardHealthPoints, cardEnergyPoints));
    }

    public int GetPartyCount()
    {
        return partyMembers.Count;
    }

    public bool CheckPartyMember(CombatCardTemplate card)
    {
        return partyMembers.Find(x=>x.CombatCardTemplate.Equals(card)) != null;
    }

    void Start()
    {

    }

    public List<PartyMember> GetPartyMembers()
    {
        return partyMembers;
    }

    public void RemovePartyMember(CombatCardTemplate cardToRemove)
    {
        partyMembers.RemoveAll(Card => { return Card.CombatCardTemplate.Equals(cardToRemove); });
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
