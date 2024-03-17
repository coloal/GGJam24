using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers")]
    [SerializeField]
    TurnManager TurnManager;
    [SerializeField]
    CardsManager CardsManager;
    [SerializeField]
    EndManager EndManager;
    [SerializeField]
    BrainManager BrainManager;
    [SerializeField]
    StoryManager StoryManager;
    [SerializeField]
    PartyManager PartyManager;

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

        DontDestroyOnLoad(gameObject);
    }

    void Start() 
    {
        AudioManager.Instance.Play(SoundNames.GameBGM);
        StartGame();
    }

    void StartGame() 
    {

        StoryCardTemplate FirstCard;
        if (!StoryManager.GetNextCardInGraph(out FirstCard))
        {
            TurnManager.StartTurn(FirstCard);
        };
        
    }

    // Maybe we need several FinishGame functions for every final that the game has
    public void FinishGame() 
    {
        Debug.Log("The game has finished! Congratulations ...or maybe not?");

    }


    public CardsManager ProvideCardsManager()
    {
        return CardsManager;
    }

    public TurnManager ProvideTurnManager()
    {
        return TurnManager;
    }
    public EndManager ProvideEndManager() {
        return EndManager;
    }

    public BrainManager ProvideBrainManager()
    {
        return BrainManager;
    }
    
    public StoryManager ProvideStoryManager()
    {
        return StoryManager;
    }

    public PartyManager ProvidePartyManager()
    {
        return PartyManager;
    }

}
