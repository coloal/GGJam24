using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    int IndexFinal;



    [SerializeField]
    List<string> FinalTexts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishGame()
    {
        Stats stats= GameManager.Instance.ProvideStatsManager().GetStats();
        if(stats.MoneyStat >= stats.StatsMaxValue) 
        {
            IndexFinal = 0;
        }
        else if (stats.ViolenceStat >= stats.StatsMaxValue)
        {
            IndexFinal = 1;
        }
        else if (stats.InfluenceStat >= stats.StatsMaxValue)
        {
            IndexFinal = 2;
        }
        else if (stats.MoneyStat <= 0)
        {
            IndexFinal = 3;
        }
        else if (stats.ViolenceStat <= 0)
        {
            IndexFinal = 4;
        }
        else if (stats.InfluenceStat <= 0)
        {
            IndexFinal = 5;
        }

        ActivateFinish();
    }

    public void FinishGameDeckEmpty() 
    {
        IndexFinal = 6;
        ActivateFinish();
    }

    private void ActivateFinish() 
    {
        Debug.Log("Final " + IndexFinal);
    }
}
