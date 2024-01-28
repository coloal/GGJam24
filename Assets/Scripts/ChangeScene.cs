using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour, IClickable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick() 
    {
        //change scene to start menu 
        Debug.Log("ist time to restart");

        SceneManager.LoadScene(ScenesNames.MainMenuScene);

    }

    public void OnMouseOver() { }
}
