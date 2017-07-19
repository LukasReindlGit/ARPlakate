
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
    

    string fileName = "";

    private bool isProcessing = false;
    private bool isFocus = false;

    public void ShareBtnPress()
    {
        if (!isProcessing)
        {            
            StartCoroutine(DoWorkflow());
        }
    }

    public void TakePictureBtnPress()
    {

        StateHandler.Instance.SetState(StateHandler.STATE.PreviewPic);

        // Disable what have to be disabled
        foreach (var d in disableDuringScreenshot)
        {
            d.SetActive(false);
        }
        
        CameraDevice.Instance.Stop();
    }

    IEnumerator DoWorkflow()
    {
        StateHandler.Instance.SetState(StateHandler.STATE.TakingScreenshot);

        yield return new WaitForSeconds(0.5f);

        isProcessing = true;

        foreach (var d in disableDuringScreenshot)
        {
            d.SetActive(false);
        }
        
        yield return StartCoroutine(CaptureScreenshot());
        yield return StartCoroutine(ShareImage());

        isProcessing = false;

        foreach (var d in disableDuringScreenshot)
        {
            d.SetActive(true);
        }
    }
    
    IEnumerator CaptureScreenshot()
    {

        fileName = GetNewFilename();

        Texture2D tex = new Texture2D(Screen.width, Screen.height);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToJPG();

        File.WriteAllBytes(Application.temporaryCachePath+"/"+fileName, bytes);
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
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + Application.temporaryCachePath + "/"+fileName);
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

        yield return new WaitForSeconds(1);
        StateHandler.Instance.SetState(StateHandler.STATE.Explore);
    }

    private void OnApplicationFocus(bool focus)
    {
        isFocus = focus;

       // StateHandler.Instance.SetState(StateHandler.STATE.Explore);
    }
    
    private string GetNewFilename()
    {
        string newName = "diePARTEI_" + UnityEngine.Random.Range(0, 99999999).ToString() + ".jpg";

        Debug.Log(newName);
        return newName;
    }
}
