using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class DynamicCam_RotateToPosition : MonoBehaviour
{

    public Camera cam;
    public Datamosh mosh;

    public Transform startPoint;
    public Transform endPoint;
    public Transform midPoint;
    public Transform lookPoint;
    Vector3 midPointRotation;

    public float rotateSpeed = 1;
    public AnimationCurve rotationCurve;
    public AnimationCurve zRotCurve;
    public AnimationCurve zShakeCurve;
    public AnimationCurve fovCurve;
    public AnimationCurve EntropyCurve;

    float fov;
    public float fovMag;
    public float zShake;
    public float EntropyMag;

    float angleToTurn;
    // Start is called before the first frame update
    void Start()
    {
        mosh = cam.GetComponent<Datamosh>();
    }

    void setTransforms()
    {
        mosh.Reset();
        startPoint.position = cam.transform.position;
        cam.transform.parent = startPoint;
        midPoint.position = startPoint.position + ((endPoint.position-startPoint.position)/2);
        midPoint.LookAt(startPoint);
        midPointRotation = midPoint.localRotation.eulerAngles;

        startPoint.parent = midPoint;
        midPoint.parent = endPoint;
        fov = cam.fieldOfView;
        
        angleToTurn = Vector3.Dot(midPoint.forward, (endPoint.forward));
        
        angleToTurn = Mathf.Acos(angleToTurn);
        Debug.Log(angleToTurn);
        
        angleToTurn = Mathf.Rad2Deg;
       
        
    }

    public void spinCam()
    {
        setTransforms();
        StartCoroutine(spinCamera());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(spinCamera());
        }
    }

    IEnumerator spinCamera()
    {    
        mosh.Glitch();
        float timer = 0;
        while (timer<1)
        {
            timer += Time.deltaTime * rotateSpeed;
            float yrot = Mathf.LerpUnclamped(0, 180, rotationCurve.Evaluate(timer));
            float zrot = Mathf.LerpUnclamped(0, 20, zRotCurve.Evaluate(timer))+(zShake*zShakeCurve.Evaluate(timer));
            float entropy = Mathf.LerpUnclamped(0, EntropyMag, EntropyCurve.Evaluate(timer));
            cam.fieldOfView = fov + ((fovCurve.Evaluate(timer)*fovMag)); ;
            midPoint.localRotation = Quaternion.Euler(midPointRotation + new Vector3(0,yrot,zrot));
            mosh.entropy = entropy;
            timer = Mathf.Clamp(timer,0,1);
            //cam.transform.LookAt(midPoint);
            yield return new WaitForSeconds(Time.deltaTime);
        }


    }
}
