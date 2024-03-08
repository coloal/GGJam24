using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndManager : MonoBehaviour
{
    [SerializeField]
    GameObject FinalGameObject;

    [SerializeField]
    TextMeshPro TextBox;

    [SerializeField]
    GameObject ButtonChangeScene;

    

    [SerializeField]
    string MaxMoneyEnding;
    [SerializeField]
    string MaxViolenceEnding;
    [SerializeField]
    string MaxInfluenceEnding;
    [SerializeField]
    string NoMoneyEnding;
    [SerializeField]
    string NoViolenceEnding;
    [SerializeField]
    string NoInfluenceEnding;
    [SerializeField]
    string EmptyDeckEnding;



    [SerializeField]
    SpriteRenderer FinalSpriteBox;
    [SerializeField]
    Sprite MaxMoneyEndingSprite;
    [SerializeField]
    Sprite MaxViolenceEndingSprite;
    [SerializeField]
    Sprite MaxInfluenceEndingSprite;
    [SerializeField]
    Sprite NoMoneyEndingSprite;
    [SerializeField]
    Sprite NoViolenceEndingSprite;
    [SerializeField]
    Sprite NoInfluenceEndingSprite;
    [SerializeField]
    Sprite EmptyDeckEndingSprite;

    string EndGameText;

    public void FinishGame()
    {
        /*
        FinalSpriteBox.gameObject.SetActive(true);
        Stats stats= GameManager.Instance.ProvideStatsManager().GetStats();
        if(stats.MoneyStat >= stats.StatsMaxValue) 
        {
            EndGameText = MaxMoneyEnding;
            PlayGoodEndingMusic();
            FinalSpriteBox.sprite = MaxMoneyEndingSprite;
        }
        else if (stats.ViolenceStat >= stats.StatsMaxValue)
        {
            EndGameText = MaxViolenceEnding;
            PlayGoodEndingMusic();
            FinalSpriteBox.sprite = MaxViolenceEndingSprite;
        }
        else if (stats.InfluenceStat >= stats.StatsMaxValue)
        {
            EndGameText = MaxInfluenceEnding;
            PlayGoodEndingMusic();
            FinalSpriteBox.sprite = MaxInfluenceEndingSprite;
        }
        else if (stats.MoneyStat <= 0)
        {
            EndGameText = NoMoneyEnding;
            PlayBadEndingMusic();
            FinalSpriteBox.sprite = NoMoneyEndingSprite;
        }
        else if (stats.ViolenceStat <= 0)
        {
            EndGameText = NoViolenceEnding;
            PlayBadEndingMusic();
            FinalSpriteBox.sprite = NoViolenceEndingSprite;
        }
        else if (stats.InfluenceStat <= 0)
        {
            EndGameText = NoInfluenceEnding;
            PlayBadEndingMusic();
            FinalSpriteBox.sprite = NoInfluenceEndingSprite;
        }

        ActivateFinish();*/
    }

    public void FinishGameDeckEmpty() 
    {
        EndGameText = EmptyDeckEnding;
        PlayBadEndingMusic();
        ActivateFinish();
        FinalSpriteBox.gameObject.SetActive(true);
        FinalSpriteBox.sprite = EmptyDeckEndingSprite;
        
    }

    private void ActivateFinish() 
    {
        TextBox.text = EndGameText;
        ButtonChangeScene.SetActive(true);
        FinalGameObject.SetActive(true);
    }

    void PlayGoodEndingMusic()
    {
        AudioManager.Instance.Play(SoundNames.PositiveFinal);
    }

    void PlayBadEndingMusic()
    {
        AudioManager.Instance.Play(SoundNames.NegativeFinal);
    }
}
