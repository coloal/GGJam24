using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatVisualManager : MonoBehaviour
{
    [Header("Scene visual configurations")]
    [SerializeField] private SpriteRenderer sceneBackgroundSpriteRenderer;
    [SerializeField] private Image combatTurnsContainerImage;

    [Header("Scene animations")]
    [Header("Move enemy card animation")]
    [SerializeField] private Transform enemyCardFinalPosition;

    [Header("Debug")]
    [SerializeField] private List<Sprite> debugTurnsSprites;

    private MoveCardAnimationComponent moveCardAnimationComponent;
    private Dictionary<string, Sprite> numberSpritesDictionary;

    void Awake()
    {
        moveCardAnimationComponent = GetComponent<MoveCardAnimationComponent>();
    }

    void Start()
    {
        InitCombatSceneVisuals();
        InitCombatSceneAudio();
    }

    private void InitCombatSceneVisuals()
    {
        BrainManager brainManager = GameManager.Instance.ProvideBrainManager();
        if (brainManager)
        {
            sceneBackgroundSpriteRenderer.sprite = brainManager.ZoneInfo.CombatBackgroundSprite;
            combatTurnsContainerImage.sprite = brainManager.ZoneInfo.CombatTurnsContainerSprite;
            InitTurnsNumberSpritesDictionary(brainManager.ZoneInfo.CombatTurnSprites);
        }
        // DEBUG
        else
        {
            InitTurnsNumberSpritesDictionary(debugTurnsSprites);
        }
    }

    private void InitCombatSceneAudio()
    {
        /*
        bool IsBoss = GameManager.Instance.ProvideBrainManager().bIsBossFight;
        List<PartyMember> members = GameManager.Instance.ProvidePartyManager().GetPartyMembers();

        GameManager.Instance.ProvideBrainSoundManager().StartCombat(members ,IsBoss);
        /**/
    }

    void InitTurnsNumberSpritesDictionary(List<Sprite> numberImages)
    {
        numberSpritesDictionary = new Dictionary<string, Sprite>();
        for (int i = 0; i < numberImages.Count; i++)
        {
            numberSpritesDictionary.Add(i.ToString(), numberImages[i]);
        }
    }

    public (Sprite, Sprite) GetTurnNumberAsSprites(int turn)
    {
        return numberSpritesDictionary.GetNumbersAsSprites(turn);
    }

    public void PlayMoveEnemyCardAnimation(GameObject enemyCardToMove, Action onAnimationEnded)
    {
        if (moveCardAnimationComponent)
        {
            moveCardAnimationComponent.StartMovingCardTowards(
                cardToMove: enemyCardToMove,
                cardFinalPosition: enemyCardFinalPosition,
                onAnimationEnded: onAnimationEnded
            );
        }
    }
}
