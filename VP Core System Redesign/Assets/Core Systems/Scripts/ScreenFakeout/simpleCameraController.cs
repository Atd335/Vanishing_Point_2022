using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleCameraController : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float yRot = 0;
    public float spd = 10;
    public float walkSpd = 1;

    // Update is called once per frame
    void Update()
    {
        yRot += Input.GetAxis("Mouse X") * spd;
        transform.rotation = Quaternion.Euler(0,yRot,0);

        transform.position += ((transform.right * Input.GetAxis("Horizontal_3D")) + (transform.forward * Input.GetAxis("Vertical_3D"))) * walkSpd * Time.deltaTime;
    }
}
