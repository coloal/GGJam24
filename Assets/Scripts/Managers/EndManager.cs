using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class EndManager : MonoBehaviour
{
    [SerializeField]
    List<string> FinalTexts;

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

    string EndGameText;

    public void FinishGame()
    {
        Stats stats= GameManager.Instance.ProvideStatsManager().GetStats();
        if(stats.MoneyStat >= stats.StatsMaxValue) 
        {
            EndGameText = MaxMoneyEnding;
            PlayGoodEndingMusic();
        }
        else if (stats.ViolenceStat >= stats.StatsMaxValue)
        {
            EndGameText = MaxViolenceEnding;
            PlayGoodEndingMusic();
        }
        else if (stats.InfluenceStat >= stats.StatsMaxValue)
        {
            EndGameText = MaxInfluenceEnding;
            PlayGoodEndingMusic();
        }
        else if (stats.MoneyStat <= 0)
        {
            EndGameText = NoMoneyEnding;
            PlayBadEndingMusic();
        }
        else if (stats.ViolenceStat <= 0)
        {
            EndGameText = NoViolenceEnding;
            PlayBadEndingMusic();
        }
        else if (stats.InfluenceStat <= 0)
        {
            EndGameText = NoInfluenceEnding;
            PlayBadEndingMusic();
        }

        ActivateFinish();
    }

    public void FinishGameDeckEmpty() 
    {
        EndGameText = EmptyDeckEnding;
        PlayBadEndingMusic();
        ActivateFinish();
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
