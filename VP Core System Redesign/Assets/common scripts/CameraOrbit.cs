using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public float speed;
    Transform t;

    void Start() {
        t = transform;
    }
    void Update()
    {
        t.localRotation = Quaternion.Euler(t.localEulerAngles + Vector3.up * speed * Time.deltaTime);
    }
}
