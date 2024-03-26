using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CreateTemporizer(Action lambda, float Seconds, MonoBehaviour m)
    {
        m.StartCoroutine(DelaySeconds(Seconds, lambda));
    }


    public static IEnumerator DelaySeconds(float Seconds, Action lambda)
    {
       yield return new WaitForSeconds(Seconds);
       lambda();
    }
}
