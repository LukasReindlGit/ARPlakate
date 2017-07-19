using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TestScalePlane : MonoBehaviour {

    [SerializeField]
    GameObject source;

    GameObject myObject;

    [SerializeField]
    Camera cam;

    [SerializeField]
    Camera srcCam;


    [SerializeField]
    RenderTexture rendTex;

    public GameObject test;

    bool wantTo = false;

	public void Perform()
    {
        wantTo = true;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (wantTo)
            StartCoroutine(CreateSnap());
    }


    private IEnumerator CreateSnap()
    {
        yield return new WaitForSeconds(5);
        cam.fieldOfView = srcCam.fieldOfView;
        GameObject g = Instantiate(source);
        g.transform.parent = source.transform.parent;
        g.transform.position = source.transform.position+Vector3.forward;
        g.transform.rotation = source.transform.rotation;
        g.transform.localScale = source.transform.localScale;
        g.transform.localScale = new Vector3(g.transform.localScale.x, g.transform.localScale.y, g.transform.localScale.z*-1);
        g.transform.parent = null;
        g.layer = 8;

        DestroyImmediate(g.GetComponent<BackgroundPlaneBehaviour>());



       Image image = CameraDevice.Instance.GetCameraImage(Image.PIXEL_FORMAT.RGB888);

        Texture2D tempTexture= new Texture2D(  image.Width,image.Height);
        image.CopyToTexture(tempTexture);


        g.GetComponent<Renderer>().material.mainTexture = tempTexture;
        
    }    
}
