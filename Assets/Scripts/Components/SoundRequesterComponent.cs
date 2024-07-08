using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRequesterComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(string eventName) 
    {
        GameManager.Instance.ProvideSoundManager().PlaySFX(eventName);
    }

    public void PlayCombatSFX()
    {
        GameManager.Instance.ProvideSoundManager().PlayCombatSFX();
    }

    public void PlayCoinSFX() 
    {
        GameManager.Instance.ProvideSoundManager().PlayCoinSFX();
    }
}
