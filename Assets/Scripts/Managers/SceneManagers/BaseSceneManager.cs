using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneManager : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    virtual protected void Init()
    {
         GameManager.Instance.CurrentSceneManager = this;
    }

}
