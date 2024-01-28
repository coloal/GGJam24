using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTextWriter : MonoBehaviour
{
    public string textToWrite;
    public bool startToWrite = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startToWrite == true && textToWrite != null)
        {

        }
    }

    void WriteTextCharByChar(string text)
    {
        textToWrite = text;
        startToWrite = true;
    }
}
