using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PlakatLoader : MonoBehaviour
{

    [SerializeField]
    Texture2D[] fallbackTextures;

    ImageTargetBehaviour imageTarget;

    [SerializeField]
    TrackableBehaviour.Status lastStatus;

    Texture2D myTexture;

    // Use this for initialization
    void Start()
    {
        imageTarget = GetComponentInParent<ImageTargetBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {

        if (imageTarget.CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {
            if (lastStatus != imageTarget.CurrentStatus)
            {
                ProvideTexture();
            }
        }
        else

        if (imageTarget.CurrentStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            HideTracker();
        }

        lastStatus = imageTarget.CurrentStatus;
    }

    private void HideTracker()
    {
        transform.position = new Vector3(-100, -100, -100);
    }

    private void ProvideTexture()
    {
        if (myTexture == null)
        {
            int index = (int)UnityEngine.Random.Range(0, fallbackTextures.Length);
            index = index % fallbackTextures.Length;
            myTexture = fallbackTextures[index];
        }
        GetComponentInChildren<Renderer>().material.mainTexture = myTexture;

    }
}
