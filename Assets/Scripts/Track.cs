using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

    [SerializeField]
    Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;
	}
}
