using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    [Header("Stat max values")]
    [SerializeField]
    [Tooltip("Max value that Stat can reach")]
    int StatsMaxValue;

    [Header("Stats initial values")]
    [SerializeField]
    [Range(0, 10)]
    int ViolenceStat;
    [SerializeField]
    [Range(0, 10)]
    int MoneyStat;
    [SerializeField]
    [Range(0, 10)]
    int InfluenceStat;

    [Header("UI for Stat bars")]
    Slider ViolenceStatBar;
    Slider MoneyStatBar;
    Slider InfluenceStatBar;

    void ModifyViolenceStat(int Amount) 
    {
        ViolenceStat += Amount;
        if (ViolenceStat >= StatsMaxValue)
        {
            ViolenceStat = StatsMaxValue;
        } 
        else if (ViolenceStat < 0)
        {
            ViolenceStat = 0;
        }
        ViolenceStatBar.value = ViolenceStat;
    }

    void ModifyMoneyStat(int Amount) 
    {
        MoneyStat += Amount;
        if (MoneyStat >= StatsMaxValue)
        {
            MoneyStat = StatsMaxValue;
        } 
        else if (MoneyStat < 0)
        {
            MoneyStat = 0;
        }
        MoneyStatBar.value = MoneyStat;
    }

    void ModifyInfluenceStat(int Amount) 
    {
        InfluenceStat += Amount;
        if (InfluenceStat >= StatsMaxValue)
        {
            InfluenceStat = StatsMaxValue;
        } 
        else if (InfluenceStat < 0)
        {
            InfluenceStat = 0;
        }
        InfluenceStatBar.value = InfluenceStat;
    }

    public void ModifyStats(int ViolencePoints, int MoneyPoints, int InfluencePoints)
    {
        ModifyViolenceStat(ViolencePoints);
        ModifyMoneyStat(MoneyPoints);
        ModifyInfluenceStat(InfluencePoints);
    }

    public bool HasAStatBeenDepletedOrCompleted()
    {
        return (ViolenceStat <= 0 || ViolenceStat >= StatsMaxValue)
            || (MoneyStat <= 0 || MoneyStat >= StatsMaxValue)
            || (InfluenceStat <= 0 || InfluenceStat >= StatsMaxValue);
    }
}
