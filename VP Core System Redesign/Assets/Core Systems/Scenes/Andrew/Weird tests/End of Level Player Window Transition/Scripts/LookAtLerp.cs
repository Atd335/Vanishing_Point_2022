using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtLerp : MonoBehaviour
{

    public Transform looker;
    public Transform thingToLookAt;
    Vector3 lerpPos;
    public float spd = 4;

	public void _Start()
    {
        
    }
    public void _Update()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        lerpPos = thingToLookAt.position;
    }

    // Update is called once per frame
    void Update()
    {
        lerpPos = Vector3.Lerp(lerpPos, thingToLookAt.position, Time.deltaTime * spd);
        looker.LookAt(lerpPos);
    }
}
