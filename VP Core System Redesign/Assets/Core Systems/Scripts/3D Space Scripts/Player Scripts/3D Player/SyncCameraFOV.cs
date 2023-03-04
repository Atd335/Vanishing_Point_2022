using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncCameraFOV : MonoBehaviour
{

    public Camera camToSyncTo;

    void LateUpdate()
    {
        GetComponent<Camera>().fieldOfView = camToSyncTo.fieldOfView;
    }
}
