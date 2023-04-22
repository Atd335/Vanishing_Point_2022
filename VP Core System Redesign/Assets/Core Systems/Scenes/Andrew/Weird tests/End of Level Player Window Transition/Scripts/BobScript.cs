using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobScript : MonoBehaviour
{
    public Transform referencePivot;

    public float masterSpdMultiplier = 1;

    public Vector3 axesMagnitude;
    public Vector3 rotMagnitude;
    public Vector3 axesSpeeds;
    public Vector3 rotSpeeds;
    
    Vector3 axesBob;
    Vector3 axesRot;

    // Start is called before the first frame update
    void Start()
    {
        axesSpeeds *= masterSpdMultiplier;
        rotSpeeds *= masterSpdMultiplier;
    }

    // Update is called once per frame
    void Update()
    {


        axesBob = new Vector3(axesMagnitude.x * ((Mathf.Sin(Time.time * axesSpeeds.x))),
                              axesMagnitude.y * ((Mathf.Sin(Time.time * axesSpeeds.y))),
                              axesMagnitude.z * ((Mathf.Sin(Time.time * axesSpeeds.z)))
                              );

        axesRot = new Vector3(rotMagnitude.x * ((Mathf.Sin(Time.time * rotSpeeds.x))),
                              rotMagnitude.y * ((Mathf.Sin(Time.time * rotSpeeds.y))),
                              rotMagnitude.z * ((Mathf.Sin(Time.time * rotSpeeds.z)))
                              );

        transform.position = referencePivot.position + axesBob;
        transform.localRotation = Quaternion.Euler(axesRot);
    }
}
