using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableHitmanSprite : MonoBehaviour, IClickable
{

    [SerializeField]
    private GameObject OriginalHM;

    public void OnClick() { }

    public void OnMouseOver() 
    {
        OriginalHM.GetComponent<HitMan>().activateSpriteOnOver(false);
    }


}
