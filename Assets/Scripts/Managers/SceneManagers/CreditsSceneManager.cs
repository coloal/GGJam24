using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSceneManager : BaseSceneManager
{
    public static CreditsSceneManager Instance;
    
    [SerializeField] 
    private TurnManager turnManager;
    [SerializeField]
    private CardsManager cardsManager;
    [SerializeField]
    private ZoneTemplate zoneTemplate;
    [SerializeField]
    private CodeGraphAsset defaultGraph;

    private GraphTypes graphType = GraphTypes.Credits;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Init();

        if (!GameManager.Instance.HasAStoryStarted())
        {
            StartStory();
        }
    }

    public void StartStory()
    {
        GameManager.Instance.SetHasAStoryStarted(true);

        GameManager.Instance.ProvideStoryManager().ChangeStory(defaultGraph, true);
        GameManager.Instance.ProvideBrainManager().ChangeZone(zoneTemplate);
        GameManager.Instance.ProvideBrainManager().SetActualGraphType(graphType);
        GameManager.Instance.ProvideSoundManager().StartGame(GameManager.Instance.ProvideBrainManager().ZoneInfo.StoryMusicZone);
        turnManager.SetZoneSprites();
        turnManager.StartTurn();
    }

    public TurnManager ProvideTurnManager()
    {
        return turnManager;
    }
    
    public CardsManager ProvideCardsManager()
    {
        return cardsManager;
    }

    
}
