using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSceneManager : BaseSceneManager
{
    public static MainGameSceneManager Instance;
    
    [SerializeField] 
    private TurnManager turnManager;
    [SerializeField]
    private CardsManager cardsManager;
    [SerializeField]
    private ZoneTemplate zoneTemplate;
    [SerializeField]
    private CodeGraphAsset defaultGraph;

    [Space]
    [SerializeField]
    private bool IsMainGame = true;

    private GraphTypes graphType = GraphTypes.Story;

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
            if (GameManager.Instance.ProvideBrainManager().bCustomGraphForCombatMode) 
            {
                defaultGraph = GameManager.Instance.ProvideBrainManager().nextGraph;
            }

            StartStory();
        }
        else
        {
            turnManager.SetZoneSprites();
        }

        GameManager.Instance.IsMainGame = IsMainGame;
    }

    public void StartStory()
    {
        GameManager.Instance.SetHasAStoryStarted(true);

        GameManager.Instance.ProvideStoryManager().ResetStory();
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
