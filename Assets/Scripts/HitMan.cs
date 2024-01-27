using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static ScenesNames;

public class HitMan : MonoBehaviour
{

    //Type of HitMan
    public HitManTypes HitManType;

    //Information of the card
    public string NameOfHitman;
    public string Description;

    //Stats
    public int ViolenceStat;
    public int MoneyStat;
    public int InfluenceStat;

    //HitMan sprite
    public Sprite HitManSprite;
    
    [SerializeField]
    private TextMeshPro BoxName;

    [SerializeField]
    private TextMeshPro BoxDescription;

    [SerializeField]
    private SpriteRenderer SpriteRenderer;

    [SerializeField]
    private HitManTemplate Data;


    // Start is called before the first frame update
    void Start()
    {
        SetDataHitMan();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDataHitMan(/*HitManTemplate Data*/) 
    {
        //Type of HitMan
        HitManType = Data.HitManType;

        //Information of the card
        NameOfHitman = Data.NameOfHitman;
        Description = Data.Description;

        //Stats
        ViolenceStat = Data.ViolenceStat;
        MoneyStat = Data.MoneyStat;
        InfluenceStat = Data.InfluenceStat;

        HitManSprite = Data.HitManSprite;

        BoxName.text = NameOfHitman;
        BoxDescription.text = Description;
        SpriteRenderer.sprite = HitManSprite;
    }

    public void OnLeftClick() 
    {
        //Se ha seleccionado este sicario
        Debug.Log("Seleccionaste a:" + NameOfHitman);
    }
}
