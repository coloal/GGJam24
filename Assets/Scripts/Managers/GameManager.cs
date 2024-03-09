using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    TurnManager TurnManager;

    [SerializeField]
    CardsManager CardsManager;

    [SerializeField]
    EndManager EndManager;

<<<<<<< HEAD
    [SerializeField]
    BrainManager BrainManager;
=======
    
>>>>>>> 6fda834b708492154ebda987702530cca0c7de60

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

    void Start() 
    {
        AudioManager.Instance.Play(SoundNames.GameBGM);
        StartGame();
    }

    void StartGame() 
    {
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
    
}
