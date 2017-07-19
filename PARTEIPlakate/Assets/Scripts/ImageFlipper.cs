using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFlipper : MonoBehaviour {
    
    private void OnEnable()
    {
        CameraSwitch.SwitchedCam += CameraSwitch_SwitchedCam;
    }

    private void OnDisable()
    {
        CameraSwitch.SwitchedCam -= CameraSwitch_SwitchedCam;
    }

    private void CameraSwitch_SwitchedCam(Vuforia.CameraDevice.CameraDirection newDir)
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    
}
