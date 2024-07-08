using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResultDrawTutorialState : ResultDrawState
{
    protected override EnemyDeckManager GetEnemyDeck()
    {
        return TutorialManager.SceneTutorial.EnemyDeck;
    }
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatManager().OverwriteCombatContext(combatContext);

        float secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForNextCombatRound;
        CombatState nextCombatState = null;

        //Al enemigo o al Player le quedan cartas
        if (GetEnemyDeck().GetNumberOfCardsInDeck() > 0
            && CombatSceneManager.Instance.ProvidePlayerDeckManager().GetNumberOfCardsInHand() > 0 )
        {
            secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForNextCombatRound;
            nextCombatState = new PickEnemyCardTutorialState();
        }
        else
        {
            secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForTossCoin;
            nextCombatState = new TossCoinTutorialState();
        }

        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: nextCombatState,
            seconds: secondsForNextProcessState
        );
    }


    public override async void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        await AttackCards(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }
        Task WaitForConversation = new Task(() => { });
        TutorialManager.SceneTutorial.StartDrawExplanation(() =>
        {
            WaitForConversation.Start();
        });
        await WaitForConversation;
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }
        await SendPlayerCombatCardToTieZone(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }
        await SendEnemyCombatCardToTieZone(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }
        PostProcess(combatContext);
    }
}
