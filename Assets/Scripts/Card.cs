using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{

    [SerializeField]
    private CardTemplate DataCard;

    [SerializeField] private TextMeshPro BoxNameOfCard;

    //Information of the card
    private string NameOfCard;
    private string Name;
    private string Gen;
    private string Background;

    //Indice del chat a reproducir se debe autoincrementar al terminar con esta carta
    private int indexChats = 0;
    private List<string> Chats;

    private List<string> LeftChoices;
    private List<string> RightChoices;

    //next cards to appear
    private List<GameObject> LeftCards;
    private List<GameObject> RightCards;

    //Incrementos/Decrementos
    private int incrementBar1; //Nombre provisional pdt
    private int incrementBar2; //Nombre provisional pdt
    private int incrementBar3; //Nombre provisional pdt


    // Start is called before the first frame update
    void Start()
    {

        //Set información del DataCard
        NameOfCard = DataCard.NameOfCard;
        Name = DataCard.Name;
        Gen = DataCard.Gen;
        Background = DataCard.Background;

        indexChats = DataCard.indexChats;
        Chats = DataCard.Chats;

        LeftChoices = DataCard.LeftChoices;
        RightChoices = DataCard.RightChoices;

        LeftCards = DataCard.LeftCards;
        RightCards = DataCard.RightCards;

        incrementBar1 = DataCard.incrementBar1; 
        incrementBar2 = DataCard.incrementBar2;
        incrementBar3 = DataCard.incrementBar3;

        //Mostrar texto en pantalla
        BoxNameOfCard.text = NameOfCard;

}

    // Update is called once per frame
    void Update()
    {
        
    }
}
