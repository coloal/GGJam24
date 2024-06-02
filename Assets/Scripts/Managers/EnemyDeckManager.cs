using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckManager : MonoBehaviour
{

    private List<CombatCard> Hand;
    private List<CombatCard> Graveyard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public CombatCard SelectRandomCard()
    {
        int idx = Random.Range(0, Hand.Count);
        return Hand[idx];
    }

    public void KillCard(CombatCard card) 
    {
        Hand.Remove(card);
    }

    public void Init(EnemyTemplate template)
    {
        template.CombatCards.ForEach(handTemplate =>
        {
            EnemyCombatCard card = new EnemyCombatCard();
            card.SetDataCard(handTemplate);
            Hand.Add(card);
            Graveyard.Add(card);
        });
    }

}
