using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResultLoseState : CombatState
{
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {

        //TODO: eliminar cartas obtenidas mediante BrainManager ??
        
        //GameManager.Instance.EndCombat(TurnResult.COMBAT_GAME_OVER);
        //throw new System.NotImplementedException();
    }

    public override void Preprocess(CombatManager.CombatContext combatContext)
    {
    }

    public override async void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        // This method should never do something, as the combat design implies
        await ReturnPlayerCardsFromHandToDeck(combatContext);
        
        PostProcess(combatContext);
    }

    async Task ReturnPlayerCardsFromHandToDeck(CombatManager.CombatContext combatContext)
    {
        List<Transform> cardInHandContainers = combatContext.GetPlayerCardInHandContainers();
        for (int i = cardInHandContainers.Count - 1; i >= 0; i--)
        {
            if (cardInHandContainers[i].childCount > 0)
            {
                CombatCard combatCard = cardInHandContainers[i].GetChild(0).GetComponent<CombatCard>();
                if (combatCard != null)
                {
                    combatCard.transform.SetParent(
                        combatContext.playerDeck.transform.parent,
                        worldPositionStays: true
                    );
                    await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                        .PlayReturnCardToDeck(
                            cardToReturn: combatCard,
                            combatContext.playerDeck.transform
                        );
                    combatCard.gameObject.SetActive(false);
                }
            }
        }
    }
}
