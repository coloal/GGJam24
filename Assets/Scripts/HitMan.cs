using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static ScenesNames;

public class HitMan : MonoBehaviour, IClickable 
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
    private SpriteRenderer SpriteRendererNonSelected;

    [SerializeField]
    private HitManTemplate Data;

    private InputActions input;
    
    [SerializeField]
    private Camera gameCamera;

    // Start is called before the first frame update
    void Start()
    {
        input = new InputActions();
        input.Default.Enable();

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

    public void OnClick() 
    {
        GameManager.Instance.ProvideTurnManager().OnHitmenSelected(HitManType);
    }

    public void OnMouseOver() 
        {
        activateSpriteOnOver(true);

        /*
        if (GameManager.Instance.ProvideTurnManager().GetCurrentGameState() == GameStates.PICK_A_HITMAN) 
        {
        }*/

        //Se ha seleccionado este sicario
        Debug.Log("encima a:" + NameOfHitman);
    }

    public void activateSpriteOnOver(bool bo) 
    {
        //SpriteRendererNonSelected.enabled = bo;

        SpriteRenderer.GetComponent<SpriteRenderer>().enabled = bo;
    }
}
