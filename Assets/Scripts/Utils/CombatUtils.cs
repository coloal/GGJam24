using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public static class CombatUtils
{
    public static int CalculateEnergy(int Turns)
    {
        return (int) Mathf.Ceil((float)Turns / 3f);
        
        /*
        float NewEnergy = Mathf ((float)Turns / 3f;
        NewEnergy += 0.3f;
        if (NewEnergy < 1f)
        {
            Energy = 1;
        }
        else
        {
            Energy = Mathf.RoundToInt(NewEnergy);
        }*/
    }
}
