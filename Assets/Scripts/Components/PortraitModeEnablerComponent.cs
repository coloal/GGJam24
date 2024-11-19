using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitModeEnablerComponent : MonoBehaviour
{
    void Awake()
    {
#if UNITY_STANDALONE
        Screen.SetResolution(1080, 1920, false);
        Screen.fullScreen = false;
#endif
    }
}
