using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeProjecterOut : MonoBehaviour
{

    public Material mat;

    float f;
    public float spd;
    void Update()
    {
        f+=Time.deltaTime*spd;
        f = Mathf.Clamp(f,0,1);

        mat.SetFloat("_FadeMag",f);
    }
}
