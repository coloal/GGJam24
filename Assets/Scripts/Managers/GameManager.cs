using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    StatsManager StatsManager;

    [SerializeField]
    CardsManager CardsManager;

    public StatsManager ProvideStatsManager()
    {
        return StatsManager;
    }

    public CardsManager ProvideCardsManager()
    {
        return CardsManager;
    }
}
