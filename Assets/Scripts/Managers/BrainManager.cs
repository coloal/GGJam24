using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrainManager : MonoBehaviour
{
    /***** DATA *****/
    private Dictionary<BrainTag, bool> BrainTagsMap;
    private Dictionary<string, string> BrainStateMap;

    /***** INITIALIZE *****/
    void Start()
    {
        BrainTagsMap = new Dictionary<BrainTag, bool>();
        BrainStateMap = new Dictionary<string, string>();

        foreach (BrainTag tag in Enum.GetValues(typeof(BrainTag)))
        {
            BrainTagsMap.Add(tag, false);
        }

        for (int i = 0; i < StateInfo.info.Count; i++)
        {
            BrainStateMap.Add(StateInfo.info[i].Item1, StateInfo.info[i].Item2[0]);
        }
    }


    /***** QUERIES *****/
    public void SetTag(BrainTag tag, bool state)
    {
        BrainTagsMap[tag] = state;
    }

    public bool GetTag(BrainTag tag)
    {
        if (BrainTagsMap.ContainsKey(tag))
        {
            return BrainTagsMap[tag];
        }

        return false;
    }

    public void SetState(int iTagState, int iState)
    {
        string TagState = StateInfo.info[iTagState].Item1;
        BrainStateMap[TagState] = StateInfo.info[iTagState].Item2[iState];
    }

    public void SetState(string TagState, string State)
    {
        BrainStateMap[TagState] = State;
    }

    public string GetState(int iTagState)
    {
        string TagState = StateInfo.info[iTagState].Item1;
        return BrainStateMap[TagState];
    }

    public string GetState(string TagState)
    {
        return BrainStateMap[TagState];
    }

    public bool IsState(int iTagState, int iState)
    {
        string TagState = StateInfo.info[iTagState].Item1;
        string ActualState = BrainStateMap[TagState];

        return ActualState.Equals(StateInfo.info[iTagState].Item2[iState]);
    }
}
