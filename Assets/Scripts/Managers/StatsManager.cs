using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [Header("Stat max values")]
    [SerializeField]
    [Tooltip("Max value that Stat can reach")]
    int StatMax;

    [Header("Stats initial values")]
    [SerializeField]
    [Range(0, 10)]
    int Stat;

    // These next functions are meant to be renamed to the specific stats that they change
    public void ModifyStat(int Amount)
    {
        Stat += Amount;
        if (Stat >= StatMax) {
            // Report that Stat has reached to the max value
        }
    }
}
