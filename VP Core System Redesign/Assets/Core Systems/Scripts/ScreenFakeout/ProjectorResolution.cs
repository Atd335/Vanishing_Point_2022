using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorResolution : MonoBehaviour
{

    void Awake()
    {
        GetComponent<Projector>().aspectRatio = (Screen.width*1f) / Screen.height;
    }

}
