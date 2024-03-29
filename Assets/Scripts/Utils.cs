using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void createTemporizer(Action lambda, float Seconds, MonoBehaviour m)
    {
        m.StartCoroutine(DelaySeconds(Seconds, lambda));
    }


    public static IEnumerator DelaySeconds(float Seconds, Action lambda)
    {
       yield return new WaitForSeconds(Seconds);
       lambda();
    }
}
