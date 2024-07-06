using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class SplashFeedbacksManager : MonoBehaviour
{
    [Header("Images fade-in fade-out configurations")]
    [SerializeField] private MMF_Player imageFadeInFadeOutPlayer;
    [SerializeField] private List<Image> imagesToShow;

    public async Task PlayFadeImages()
    {
        MMF_ImageAlpha imageFadeInFadeOutFeedbacks = imageFadeInFadeOutPlayer.GetFeedbackOfType<MMF_ImageAlpha>();
        if (imageFadeInFadeOutFeedbacks != null)
        {
            foreach (Image imageToShow in imagesToShow)
            {
                imageFadeInFadeOutFeedbacks.BoundImage = imageToShow;
                await imageFadeInFadeOutPlayer.PlayFeedbacksTask();
            }
        }
    }
}
