using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    StatsManager StatsManager;

    public StatsManager ProvideStatsManager()
    {
        return StatsManager;
    }
}
