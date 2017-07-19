using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestState : MonoBehaviour {

    public StateHandler.STATE state;

    StateHandler.STATE lastState;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(state!= lastState)
        {
            StateHandler.State = state;
        }

        lastState = state;
	}
}
