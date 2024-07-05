using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinComponent : MonoBehaviour
{
    // Heads equals -> Scale == 1
    [SerializeField] private Image headsImage;
    // Tails equals -> Scale == -1
    [SerializeField] private Image tailsImage;

    public void FlipHeads()
    {
        transform.localScale = new Vector2(
            Mathf.Abs(transform.localScale.x),
            transform.localScale.y
        );
    }

    public void FlipTails()
    {
        transform.localScale = new Vector2(
            - Mathf.Abs(transform.localScale.x),
            transform.localScale.y
        );
    }

    public void FlipTo(CoinFlipResult coinFlip)
    {
        if (coinFlip == CoinFlipResult.Heads)
        {
            FlipHeads();
        }
        else
        {
            FlipTails();
        }
    }

    public Image GetCoinFace(CoinFlipResult coinFlipResult)
    {
        return coinFlipResult == CoinFlipResult.Heads ? headsImage : tailsImage;
    }
}
