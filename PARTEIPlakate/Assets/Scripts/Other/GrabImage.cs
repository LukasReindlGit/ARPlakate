using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class GrabImage : MonoBehaviour {

    [SerializeField]
    Camera cam;

    [SerializeField]
    Camera disableCam;

    [SerializeField]
    GameObject target;

    [SerializeField]
    GameObject setToDebug;
    
    [SerializeField]
    SharePicture share;

    List<GameObject> toBeDestroyed = new List<GameObject>();

    public void Perform()
    {
        CreateSnap();
        //share.ShareBtnPress();
        share.OnFinishedShare += Share_OnFinishedShare;
    }

    private void Share_OnFinishedShare()
    {
        ReleaseSnap();
    }

    private void CreateSnap()
    {
        //Texture2D tex = target.GetComponent<Renderer>().material.mainTexture as Texture2D;

        //Texture2D tempTex = new Texture2D(tex.width, tex.height);

        //tempTex.SetPixels(tex.GetPixels());
        //tempTex.Apply();

        //GameObject g = Instantiate(target);
        //g.transform.parent = target.transform.parent;
        //g.transform.position = target.transform.position;
        //g.transform.rotation = target.transform.rotation;
        //g.transform.localScale = target.transform.localScale;
        //g.transform.parent = null;

        //DestroyImmediate(g.GetComponent<BackgroundPlaneBehaviour>());

        //g.GetComponent<Renderer>().material.mainTexture = tempTex;
        //setToDebug.GetComponent<Renderer>().material.mainTexture = tempTex;

        cam.gameObject.SetActive(true);
        SetCamValues(cam);
      //  disableCam.gameObject.SetActive(false);

      //  toBeDestroyed.Add(g);
    }

    private void SetCamValues(Camera cam)
    {
        cam.fieldOfView = disableCam.fieldOfView;
    }

    private void ReleaseSnap()
    {

        cam.gameObject.SetActive(false);
        disableCam.gameObject.SetActive(true);

        foreach (GameObject g in toBeDestroyed)
        {
            Destroy(g);
        }
        toBeDestroyed.Clear();
    }
}
