using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectWiggleMat : MonoBehaviour
{

    public Material mat;
    public Vector2 mag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mat.SetFloat("_PerlinOffsetX", Time.time * mag.x);
        mat.SetFloat("_PerlinOffsetY", Time.time * mag.y);
    }
}
