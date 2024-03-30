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

    // (r,g,b,a) are in [0, 255] values
    public static Color GetNormalizedColor(int r, int g, int b, int alpha = 255)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f, alpha / 255.0f);
    }
}
