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

    // These next functions are meant to be renamed to the specific stats that they change
    public void ModifyStats(int ViolenceStat, int MoneyStat, int InfluenceStat)
    {
        //Stat += Amount;
        //if (Stat >= StatMax) {
            // Report that Stat has reached to the max value
        //}
    }

    void ModifyViolenceStat(int Amount) {
        ViolenceStat += Amount;
        if (ViolenceStat >= StatsMaxValue)
        {
            // Report that Stat has reached to the max value
            ViolenceStatBar.value = StatsMaxValue;
        } 
        else 
        {
            ViolenceStatBar.value = ViolenceStat;
        }
    }

    void ModifyMoneyStat(int Amount) {
        MoneyStat += Amount;
        if (MoneyStat >= StatsMaxValue)
        {
            // Report that Stat has reached to the max value
            MoneyStatBar.value = StatsMaxValue;
        } 
        else 
        {
            MoneyStatBar.value = MoneyStat;
        }
    }

    void ModifyInfluenceStat(int Amount) {
        InfluenceStat += Amount;
        if (InfluenceStat >= StatsMaxValue)
        {
            // Report that Stat has reached to the max value
            InfluenceStatBar.value = StatsMaxValue;
        } 
        else 
        {
            InfluenceStatBar.value = MoneyStat;
        }
    }
}
