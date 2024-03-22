using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrainManager : MonoBehaviour
{
    /***** DATA *****/
    private Dictionary<BrainTag, bool> BrainTagsMap;
    private Dictionary<NumericTags, int> BrainNumericMap;
    private Dictionary<string, string> BrainStateMap;

    /***** INITIALIZE *****/
    void Start()
    {
        InitializeData();
    }

    void InitializeData()
    {
        if (BrainTagsMap == null)
        {
            BrainTagsMap = new Dictionary<BrainTag, bool>();
            foreach (BrainTag tag in Enum.GetValues(typeof(BrainTag)))
            {
                BrainTagsMap.Add(tag, false);
            }
        }

        if (BrainNumericMap == null)
        {
            BrainNumericMap = new Dictionary<NumericTags, int>();
            foreach (NumericTags tag in Enum.GetValues(typeof(NumericTags)))
            {
                BrainNumericMap.Add(tag,0);
            }
        }

        if (BrainStateMap == null)
        {
            BrainStateMap = new Dictionary<string, string>();

            for (int i = 0; i < StateInfo.info.Count; i++)
            {
                BrainStateMap.Add(StateInfo.info[i].Item1, StateInfo.info[i].Item2[0]);
            }
        }
    }

    /***** ACTIONS *****/
    public void ExecuteActions(List<Option> Actions)
    {
        foreach (Option action in Actions)
        {
            switch (action.TagType)
            {
                case BrainTagType.Bool:
                    //action.BrainBoolTagAction.Invoke(action.BoolTag, action.NewValue);
                    SetTag(action.BoolTag, action.NewValue);
                    break;
                case BrainTagType.Numeric:
                    //action.BrainNumericTagAction.Invoke(action.NumericTag, action.Increment);
                    IncrementNumericTag(action.NumericTag, action.Increment);

                    break;
                case BrainTagType.State:
                    //action.BrainStateIntTagAction.Invoke(action.StateTuple.selectedTag, action.StateTuple.selectedTagState);
                    SetState(action.TagState, action.NewState);
                    break;
                case BrainTagType.Combat:
                    //Enter in combat
                    Debug.Log("Combateeee");
                    GameObject a = GameManager.Instance.ProvideCardsManager().SpawnCombatCard();
                    break;
            }
        }
    }

    public void ExecuteActions(Option action)
    {
        switch (action.TagType)
        {
            case BrainTagType.Bool:
                //action.BrainBoolTagAction.Invoke(action.BoolTag, action.NewValue);
                SetTag(action.BoolTag, action.NewValue);
                break;
            case BrainTagType.Numeric:
                //action.BrainNumericTagAction.Invoke(action.NumericTag, action.Increment);
                IncrementNumericTag(action.NumericTag, action.Increment);

                break;
            case BrainTagType.State:
                //action.BrainStateIntTagAction.Invoke(action.StateTuple.selectedTag, action.StateTuple.selectedTagState);
                SetState(action.TagState, action.NewState);
                break;
            case BrainTagType.Combat:
                //Enter in combat
                Debug.Log("Combateeee");
                GameObject a = GameManager.Instance.ProvideCardsManager().SpawnCombatCard();
                break;
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

    public void SetNumericTag(NumericTags tag, int value)
    {
        BrainNumericMap[tag] = value;
    }
    public void IncrementNumericTag(NumericTags tag, int value)
    {
        BrainNumericMap[tag] += value;
        if (BrainNumericMap[tag] < 0)
        {
            BrainNumericMap[tag] = 0;
        }
    }

    public int GetNumericTag(NumericTags tag)
    {
        if (BrainNumericMap.ContainsKey(tag))
        {
            return BrainNumericMap[tag];
        }
        return 0;
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
        if (BrainStateMap == null)
        {
            InitializeData();
        }
        string TagState = StateInfo.info[iTagState].Item1;
        string ActualState = BrainStateMap[TagState];

        return ActualState.Equals(StateInfo.info[iTagState].Item2[iState]);
    }
}
