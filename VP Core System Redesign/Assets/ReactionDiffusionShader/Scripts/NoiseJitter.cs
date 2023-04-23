using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseJitter : MonoBehaviour
{
    public Material camMat;

    Vector2 offset;

    float timer;
    public float freq = .05f;
    public float noiseStrength = .3f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timer = Mathf.Clamp(timer,0,freq);
        if (timer == freq)
        {
            offset = new Vector2(
                Random.Range(0,1f),
                Random.Range(0,1f)
                );
            camMat.SetFloat("_NoiseStrength",noiseStrength);
            camMat.SetVector("_NoiseOffset",offset);
            timer = 0;
        }
        
    }
}
