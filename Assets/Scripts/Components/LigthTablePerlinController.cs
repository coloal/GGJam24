using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LigthTablePerlinController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image ligthObject;

    bool isFlickering = false;
    float currentTime = 0;
    float totalTime = 0;
    
    

    void Start()
    {
        RecursiveFlickering();
    }

    private void Update()
    {
        SetAlpha(Mathf.PerlinNoise1D(currentTime*4));
        currentTime += Time.deltaTime;
        if (currentTime >= totalTime)
        {
            isFlickering=false;
            SetAlpha(1f);
        }
        
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
        totalTime = Random.Range(2f,3.5f);
        currentTime = 0;
        isFlickering = true;
    }

  

   

    private void SetAlpha(float alpha)
    {
        if(alpha < 0.5)
        {
            alpha -= 0.1f;
        }
        else
        {
            alpha += 0.1f;
        }
        ligthObject.color = new Color(ligthObject.color.r, ligthObject.color.g, ligthObject.color.b, alpha);
    }

}
