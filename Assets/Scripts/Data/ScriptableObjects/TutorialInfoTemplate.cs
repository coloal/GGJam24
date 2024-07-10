using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialInfoTemplate", menuName = "Tutorial Temnplate")]
public class TutorialInfoTemplate : ScriptableObject
{
    [SerializeField] private List<CombatCardTemplate> tutorialDeck;
    [SerializeField] private EnemyTemplate tutorialEnemy;
    [SerializeField] private List<string> initialConversation;
    [SerializeField] private List<string> cardExplanation;
    [SerializeField] private List<string> bookExplanation;
    [SerializeField] private List<string> enemyCardExplanationPreShow;
    [SerializeField] private List<string> enemyCardExplanationWhileShow;

    [SerializeField] private List<string> enemyPlaysCardExplanationViolenceWin;
    [SerializeField] private List<string> playerPlaysCardExplanationInfluenceWin;

    [SerializeField] private List<string> enemyPlaysCardExplanationViolenceLose;
    [SerializeField] private List<string> playerPlaysCardExplanationMoneyLose;

    [SerializeField] private List<string> enemyPlaysCardExplanationMoneyDraw;
    [SerializeField] private List<string> playerPlaysCardExplanationMoneyDraw;

    [SerializeField] private List<string> enemyPlaysCardExplanationInfluenceDraw;
    [SerializeField] private List<string> playerPlaysCardExplanationInfluenceDraw;

    [SerializeField] private List<string> enemyPlaysCardExplanationViolenceDraw;
    [SerializeField] private List<string> playerPlaysCardExplanationViolenceDraw;

    [SerializeField] private List<string> winExplanation;
    [SerializeField] private List<string> loseExplanation;
    [SerializeField] private List<string> drawExplanationFirst;
    [SerializeField] private List<string> drawExplanationSecond;
    [SerializeField] private List<string> drawExplanationThird;
    [SerializeField] private List<string> coinCardExplanation;
    [SerializeField] private List<string> coinExplanation;
    [SerializeField] private List<string> battleResultExplanation;

    [SerializeField] private List<string> pickWinCardExplanation;
    [SerializeField] private List<string> endExplanation;

    [SerializeField] private List<string> playerTrippingViolence;
    [SerializeField] private List<string> playerTrippingViolence_1;
    [SerializeField] private List<string> playerTrippingViolence_2;
    [SerializeField] private List<string> playerTrippingMoney;
    [SerializeField] private List<string> playerTrippingMoney_1;
    [SerializeField] private List<string> playerTrippingMoney_2;
    [SerializeField] private List<string> playerTrippingInfluence;
    [SerializeField] private List<string> playerTrippingInfluence_1;
    [SerializeField] private List<string> playerTrippingInfluence_2;

   


    public List<string> InitialConversation => initialConversation;

    public List<string> CardExplanation => cardExplanation;
    public List<string> BookExplanation => bookExplanation;
    public List<string> EnemyCardExplanationPreShow => enemyCardExplanationPreShow;
    public List<string> EnemyCardExplanationWhileShow => enemyCardExplanationWhileShow;

    public List<string> EnemyPlaysCardExplanationViolenceWin => enemyPlaysCardExplanationViolenceWin;
    public List<string> PlayerPlaysCardExplanationInfluenceWin => playerPlaysCardExplanationInfluenceWin;

    public List<string> EnemyPlaysCardExplanationMoneyLose => enemyPlaysCardExplanationViolenceLose;
    public List<string> PlayerPlaysCardExplanationViolenceLose => playerPlaysCardExplanationMoneyLose;

    public List<string> EnemyPlaysCardExplanationMoneyDraw => enemyPlaysCardExplanationMoneyDraw;
    public List<string> PlayerPlaysCardExplanationMoneyDraw => playerPlaysCardExplanationMoneyDraw;

    public List<string> EnemyPlaysCardExplanationInfluenceDraw => enemyPlaysCardExplanationInfluenceDraw;
    public List<string> PlayerPlaysCardExplanationInfluenceDraw => playerPlaysCardExplanationInfluenceDraw;

    public List<string> EnemyPlaysCardExplanationViolenceDraw => enemyPlaysCardExplanationViolenceDraw;
    public List<string> PlayerPlaysCardExplanationViolenceDraw => playerPlaysCardExplanationViolenceDraw;
    public List<string> WinExplanation => winExplanation;
    public List<string> LoseExplanation => loseExplanation;
    public List<string> DrawExplanationFirst => drawExplanationFirst;
    public List<string> DrawExplanationSecond => drawExplanationSecond;
    public List<string> DrawExplanationThird => drawExplanationThird;
    public List<string> CoinCardExplanation => coinCardExplanation;
    public List<string> CoinExplanation => coinExplanation;
    public List<string> BattleResultExplanation => battleResultExplanation;
    public List<string> PickWinCardExplanation => pickWinCardExplanation;
    public List<string> EndExplanation => endExplanation;

    public List<CombatCardTemplate> TutorialDeck => tutorialDeck;
    public EnemyTemplate TutorialEnemy => tutorialEnemy;

    public List<string> PlayerTrippingViolence => playerTrippingViolence;
    public List<string> PlayerTrippingViolence_1 => playerTrippingViolence_1;
    public List<string> PlayerTrippingViolence_2 => playerTrippingViolence_2;
    public List<string> PlayerTrippingMoney => playerTrippingMoney;
    public List<string> PlayerTrippingMoney_1 => playerTrippingMoney_1;
    public List<string> PlayerTrippingMoney_2 => playerTrippingMoney_2;
    public List<string> PlayerTrippingInfluence => playerTrippingInfluence;
    public List<string> PlayerTrippingInfluence_1 => playerTrippingInfluence_1;
    public List<string> PlayerTrippingInfluence_2 => playerTrippingInfluence_2;

}
