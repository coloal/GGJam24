using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    // Lists of cards to be added to the game
    [SerializeField]
    GameObject CardPrefab;

    [SerializeField]
    GameObject CombatCardPrefab;

    /*deprecated ????*/
    [SerializeField]
    List<StoryCardTemplate> DataCardsList;

    //For testing by Psyduck(adri :) )
    [SerializeField] private CombatCardTemplate CartaTESTING;

    [SerializeField]
    private bool isSequential;

    [SerializeField]
    private Transform CardSpawnerOrigin;

    private int indexNextCard = 0;

    public GameObject SpawnNextCard(StoryCardTemplate nextCard)
    {
        GameObject newCard = Instantiate(CardPrefab, CardSpawnerOrigin.position, Quaternion.identity);

        if(nextCard != null)
        {
            newCard.GetComponent<StoryCard>().SetDataCard(nextCard);
        }
        
        //indexNextCard++;
        return newCard;
    }

    public GameObject SpawnCombatCard()
    {
        GameObject newCard = Instantiate(CombatCardPrefab, new Vector3 (-6,0,0), Quaternion.identity);

        if (CartaTESTING != null)
        {
            newCard.GetComponent<CombatCard>().SetDataCard(CartaTESTING);
        }

        //indexNextCard++;
        return newCard;
    }

    public bool IsDeckEmpty()
    {
        return indexNextCard == DataCardsList.Count;
    }
}
