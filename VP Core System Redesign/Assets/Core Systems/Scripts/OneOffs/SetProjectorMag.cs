using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetProjectorMag : MonoBehaviour
{
    public Material mat;
    private void Start()
    {
        mat.SetFloat("_FadeMag", 0);
    }
}
