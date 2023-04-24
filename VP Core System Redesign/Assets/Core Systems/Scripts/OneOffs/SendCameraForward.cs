using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendCameraForward : MonoBehaviour
{

    bool b;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float spd;
    public void sendForward(float _spd)
    {
        spd = _spd;
        b = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!b) { return; }

        transform.position += transform.forward * Time.deltaTime * spd;
    }
}
