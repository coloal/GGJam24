using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrainManager : MonoBehaviour
{
    /***** DATA *****/
    private Dictionary<Tag, bool> TagsMap;


    /***** INITIALIZE *****/
    void Start()
    {
        TagsMap = new Dictionary<Tag, bool>();

        for (int i = 0; i < (int)Tag.Null; i++)
        {
            TagsMap.Add((Tag)i, true);
        }
    }


    /***** QUERIES *****/
    public void SetTag(Tag tag, bool state)
    {
        TagsMap[tag] = state;
    }

    public bool GetTag(Tag tag)
    {
        if (TagsMap.ContainsKey(tag))
        {
            return TagsMap[tag];
        }
        return false;
    }

    

}
