using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers")]
    [SerializeField]
    AudioManager AudioManager;
    [SerializeField]
    TurnManager TurnManager;
    [SerializeField]
    CardsManager CardsManager;
    [SerializeField]
    EndManager EndManager;
    [SerializeField]
    BrainManager BrainManager;
    [SerializeField]
    BrainSoundManager BrainSoundManager;
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
        StartGame();
    }

    void StartGame()
    {
        BrainSoundManager.StartGame();
        TurnManager.StartTurn();   
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
    public AudioManager ProvideAudioManager()
    {
        return AudioManager;
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

    public BrainSoundManager ProvideBrainSoundManager()
    {
        return BrainSoundManager;
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
