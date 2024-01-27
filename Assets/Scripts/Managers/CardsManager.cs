using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    // Lists of cards to be added to the game
    [SerializeField]
    GameObject CardPrefab;

    [SerializeField]
    List<CardTemplate> DataCardsList;

    [SerializeField]
    private bool isSequential;

    [SerializeField]
    private int indexNextCard = 0;

    //public static TurnsManager Instance; // A static reference to the GameManager instance

    public GameObject LastCard;
    
    void Start()
    {
        SpawnNextCard();
    }


    public GameObject SpawnNextCard()
    {
        CardPrefab.GetComponent<Card>().SetDataCard(DataCardsList[indexNextCard]);
        GameObject newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        
        indexNextCard++;

        return newCard;
    }


   

}
