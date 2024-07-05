using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPageTutorial : MonoBehaviour
{
    public GameObject shownObject;
    public GameObject hideObject;
   public void TurnPage() {
    shownObject.SetActive(true);
    hideObject.SetActive(false);

   }
}
