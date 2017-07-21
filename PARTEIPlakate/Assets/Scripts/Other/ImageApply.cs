using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ImageApply : MonoBehaviour, ITrackableEventHandler {

    [SerializeField]
    Renderer visual;

    bool gotTexture = false;

    void Start()
    {
        if (visual == null)
        {
            visual = GetComponent<Renderer>();
        }
        
        GetComponentInParent<ImageTargetBehaviour>().RegisterTrackableEventHandler(this);
        
    }        

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        Debug.Log("NEW STATE: " + newStatus.ToString());

        if(!gotTexture && newStatus == TrackableBehaviour.Status.TRACKED)
        {
            InitTexture();
        }
    }

    private void InitTexture()
    {
        visual.material.mainTexture = TextureManager.GetRandomTexture();
        gotTexture = true;
    }
}
