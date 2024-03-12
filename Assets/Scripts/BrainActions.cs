using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class BrainActions : MonoBehaviour
{
    BrainTagAction action;
    private BrainBoolTagAction BoolAction;
    private BrainNumericTagAction NumericAction;
    private BrainStateTagAction StateAction;
    public string BrainTag;

    // Start is called before the first frame update
    void Start()
    {
        /*
        BoolAction = new BrainBoolTagAction();
        NumericAction = new BrainNumericTagAction();
        StateAction = new BrainStateTagAction();
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBrainTagAction(string BrainTag, bool value)
    {
        //GameManager.Instance.ProvideBrainManager().SetTag(tag,value);
    }

    public void example(string a)
    {
    }

    public void SetBrainBoolTag(BrainTag tag, bool value)
    {
        GameManager.Instance.ProvideBrainManager().SetTag(tag, value);
    }
    public void SetBrainNumericTag(NumericTags tag, int increment)
    {
        GameManager.Instance.ProvideBrainManager().IncrementNumericTag(tag, increment);
    }
    public void SetBrainStateTag(string tag, string newState)
    {
        GameManager.Instance.ProvideBrainManager().SetState(tag,newState);
    }
}

[Serializable]
public class BrainTagAction : UnityEvent<string> {}

[Serializable]
public class BrainBoolTagAction : UnityEvent<BrainTag, bool> { }

[Serializable]
public class BrainNumericTagAction : UnityEvent<NumericTags, int> { }

[Serializable]
public class BrainStateTagAction : UnityEvent<string, string> { }


[Serializable]
public class BrainTupleTagSO : ScriptableObject
{
    public BrainTag BrainTag;
    public bool value;
}

[Serializable]
public class BrainTupleTag
{
    public BrainTag BrainTag;
    public bool value;
}