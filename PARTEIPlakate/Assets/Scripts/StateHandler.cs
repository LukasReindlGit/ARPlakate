using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System;

public class StateHandler : MonoBehaviour
{

    public enum STATE { Explore, PreviewPic, TakingScreenshot };

    private static STATE state = STATE.Explore;
    public static STATE State { get { return state; } set { state = value; Instance.OnChangedState(value); } }


    public        static StateHandler Instance;

    public OnlyOnObjects[] objectsToHandle;

    private void Awake()
    {
        Instance = this;
    }

    public void SetState(STATE newState)
    {
        State = newState;
    }

    // Use this for initialization
    void Start()
    {
        State = STATE.Explore;
    }

    void OnChangedState(STATE newState)
    {
        foreach(var v in objectsToHandle)
        {
            v.Update(newState);
        }
    }
}

[Serializable]
public class OnlyOnObjects
{
    public string name;
    public GameObject[] objects;
    public StateHandler.STATE[] statesOnVisible;

    internal void Update(StateHandler.STATE newState)
    {
        foreach(GameObject g in objects)
        {
            g.SetActive( contains(newState));
        }
    }

    bool contains(StateHandler.STATE state)
    {
        foreach (StateHandler.STATE s in statesOnVisible)
        {
            if (s == state)
                return true;
        }
        return false;
    }
}
