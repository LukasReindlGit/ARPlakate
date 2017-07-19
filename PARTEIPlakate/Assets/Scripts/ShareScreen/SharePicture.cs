
using UnityEngine;
using System.Collections;
using System.IO;
using Vuforia;
using System;

public class SharePicture : MonoBehaviour
{

    public delegate void DefaultDelegate();
    public event DefaultDelegate OnFinishedShare;

    public GameObject[] disableDuringScreenshot;
    public GameObject backplane;
    GameObject tmpBackplane;

    public Material emptyMat;


    private bool isProcessing = false;
    private bool isFocus = false;

    public void ShareBtnPress()
    {
        if (!isProcessing)
        {
            StartCoroutine(DoWorkflow());
        }
    }

    IEnumerator DoWorkflow()
    {
        Debug.Log("Starting workflow");

        isProcessing = true;

        foreach (var d in disableDuringScreenshot)
        {
            d.SetActive(false);
        }


        yield return new WaitForEndOfFrame();

        CameraDevice.Instance.Stop();

       // SwitchBackPlanes();

        yield return StartCoroutine(CaptureScreenshot2());
        yield return StartCoroutine(ShareImage());

        isProcessing = false;

        foreach (var d in disableDuringScreenshot)
        {
            d.SetActive(true);
        }
    }

    private void SwitchBackPlanes()
    {
        if(tmpBackplane!=null)
        {
            Destroy(tmpBackplane);
        }

        tmpBackplane = new GameObject();
        tmpBackplane.name = "TEMP_BGPlane";

        // Set size pos rot
        tmpBackplane.transform.parent = backplane.transform.parent;
        tmpBackplane.transform.rotation = backplane.transform.rotation;
        tmpBackplane.transform.position = backplane.transform.position;
        tmpBackplane.transform.localScale = backplane.transform.localScale;
        tmpBackplane.transform.parent = null;

        // MESH: 
        Mesh myMesh = new Mesh();
        Mesh tmpMesh = backplane.GetComponent<MeshFilter>().sharedMesh;
        myMesh.vertices = tmpMesh.vertices;
        myMesh.triangles = tmpMesh.triangles;
        myMesh.uv = tmpMesh.uv;


        tmpBackplane.AddComponent<MeshFilter>().sharedMesh = myMesh;

        // TEST: 
       // tmpBackplane.transform.position = new Vector3(0,0,300);

        // Material
        MeshRenderer rend = tmpBackplane.AddComponent<MeshRenderer>();
        rend.material = new Material(emptyMat);
        // rend.material.mainTexture = backplane.GetComponent<Renderer>().material.mainTexture;

        // rend.material.mainTexture = GetWebcamTexture();

        Texture2D tex = new Texture2D(Screen.width, Screen.height);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();

        rend.material.mainTexture = tex;


        backplane.SetActive(false);
        
    }

    private Texture GetWebcamTexture()
    {
        WebCamTexture tex = new WebCamTexture();
        tex.Play();

        return tex;
    }

    IEnumerator CaptureScreenshot()
    {
        CameraDevice.Instance.Stop();

        yield return new WaitForEndOfFrame();

        string screenshotName = "diePARTEI.jpg";//  + System.DateTime.Today.ToString() + ".png";

        Application.CaptureScreenshot(screenshotName, 2);
        string destination = Path.Combine(Application.persistentDataPath, screenshotName);

        yield return new WaitForSecondsRealtime(0.3f);
    }

    IEnumerator CaptureScreenshot2()
    {
        
        Texture2D tex = new Texture2D(Screen.width, Screen.height);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToJPG();

        File.WriteAllBytes(Application.temporaryCachePath+"/diePARTEI.jpg", bytes);
        Debug.Log("PATH: " + Application.temporaryCachePath);
        yield return null;
    }


    IEnumerator ShareImage()
    {
        if (!Application.isEditor)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + Application.temporaryCachePath + "/diePARTEI.jpg");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"),
                uriObject);
           // intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Can you beat my score?");
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser",
                intentObject, "Share it! Tu es!");
            currentActivity.Call("startActivity", chooser);

            yield return new WaitForSecondsRealtime(1);
        }

        yield return new WaitUntil(() => isFocus);

        
        
        if(OnFinishedShare!=null)
        {
            OnFinishedShare.Invoke();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }
}
