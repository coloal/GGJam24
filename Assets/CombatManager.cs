using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private PartyManager PartyManager;

    void Start()
    {
        SetUpManagers();
    }

    void SetUpManagers()
    {
        PartyManager = GameManager.Instance.ProvidePartyManager();
    }
}
