using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LigthTableController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image ligthObject;

    bool isFlickering = false;
    int currentTimes = 0;
    int totalTimes = 0;

    

    void Start()
    {
        RecursiveFlickering();
    }

    public void RecursiveFlickering()
    {
        GameUtils.CreateTemporizer(() =>
        {
            if(Random.Range(0f,1f) <= 0.1f)
            {
                StartFlickering();
            }
            RecursiveFlickering();
        }, 5f, this);
    }

    public void StartFlickering()
    {
        if (isFlickering) return;
        totalTimes = Random.Range(1,4);
        currentTimes = 0;
        isFlickering = true;
        Flicker();
    }

    private void Flicker()
    {
        
        if (currentTimes < totalTimes)
        {
            SetAlpha(getOffAlpha());
            GameUtils.CreateTemporizer(() =>
            {
                UnFlicker();
            }, getFlickDuration(currentTimes), this);
        }
        else
        {
            isFlickering = false;
        }

    }

   

    private void UnFlicker()
    {
        
        if (currentTimes < totalTimes)
        {
            SetAlpha(getOnAlpha());
            GameUtils.CreateTemporizer(() =>
            {
                Flicker();
            }, getOnDuration(currentTimes), this);
            currentTimes++;
        }
    }
    private void SetAlpha(float alpha)
    {
        ligthObject.color = new Color(ligthObject.color.r, ligthObject.color.g, ligthObject.color.b, alpha);
    }

    private float getOnDuration(int currentTime)
    {
        switch (currentTime)
        {
            case 0:
                return Random.Range(0.1f, 0.2f);
            default:
                return Random.Range(0.05f, 0.15f);
        }
    }

    private float getFlickDuration(int currentTime)
    {
        
        switch( currentTime )
        {
            case 0:
                return Random.Range(0.5f, 1f);
            default:
                return Random.Range(0.05f, 0.1f);
        }
    }


    private float getOnAlpha()
    {
        switch(totalTimes - currentTimes)
        {
            case 1:
                return 1f;
            default:
                return Random.Range(0.8f, 0.9f);
        }
     
    }

    private float getOffAlpha()
    {
        if (currentTimes == 0) return 0f;
        return Random.Range(0.3f, 0.6f);
    }

}
