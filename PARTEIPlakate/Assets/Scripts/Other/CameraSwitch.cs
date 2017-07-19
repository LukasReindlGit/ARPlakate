using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CameraSwitch : MonoBehaviour
{
    public delegate void CamUpdate(CameraDevice.CameraDirection newDir);

    public static event CamUpdate SwitchedCam;

    public void SwitchCamera()
    {
        CameraDevice camDev = CameraDevice.Instance;

        CameraDevice.CameraDirection dirToUse;
        CameraDevice.CameraDirection oldDir = camDev.GetCameraDirection();

        switch (oldDir)
        {
            case CameraDevice.CameraDirection.CAMERA_DEFAULT:
                dirToUse = CameraDevice.CameraDirection.CAMERA_FRONT;
                break;
            case CameraDevice.CameraDirection.CAMERA_BACK:
                dirToUse = CameraDevice.CameraDirection.CAMERA_FRONT;
                break;
            case CameraDevice.CameraDirection.CAMERA_FRONT:
                dirToUse = CameraDevice.CameraDirection.CAMERA_BACK;
                
                break;
            default:
                dirToUse = CameraDevice.CameraDirection.CAMERA_DEFAULT;
                break;
        }

        camDev.Stop();
        camDev.Deinit();

        camDev.Init(dirToUse);
        camDev.Start();

        if(SwitchedCam!=null)
        {
            SwitchedCam.Invoke(dirToUse);
        }
    }
}
