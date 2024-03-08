using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static ScenesNames;
using Unity.VisualScripting.Antlr3.Runtime;

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

    [SerializeField]
    private List<HitMan> OtherHitmans;

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
        //SpriteRenderer.sprite = HitManSprite;
    }

    public void OnClick() 
    {
        //if(GameManager.Instance.ProvideTurnManager().GetCurrentGameState() == GameStates.PICK_A_HITMAN) GameManager.Instance.ProvideTurnManager().OnHitmenSelected(HitManType);
    }

    public void OnMouseOver() 
        {
        //if (GameManager.Instance.ProvideTurnManager().GetCurrentGameState() == GameStates.PICK_A_HITMAN) activateSpriteOnOver(true);
        //Se ha seleccionado este sicario
        Debug.Log("encima a:" + NameOfHitman);
    }

    public void activateSpriteOnOver(bool bo) 
    {
        //SpriteRendererNonSelected.enabled = bo;

        SpriteRenderer.GetComponent<SpriteRenderer>().enabled = bo;
        if(bo)
        {
            for (int i = 0; i < OtherHitmans.Count; i++)
            {
                OtherHitmans[i].activateSpriteOnOver(false);
            }
        }
        
    }
}
