using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    StatsManager StatsManager;

    [SerializeField]
    TurnsManager TurnsManager;

    public StatsManager ProvideStatsManager()
    {
        return StatsManager;
    }

    public TurnsManager ProvideTurnsManager()
    {
        return TurnsManager;
    }
}
