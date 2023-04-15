using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class setProjectorTexture : MonoBehaviour
{

    public Texture2D tex;
    public Texture2D defaultNull;
    string imgPath;

    private void Awake()
    {
        imgPath = System.AppDomain.CurrentDomain.BaseDirectory;
        imgPath = imgPath.Substring(0, imgPath.Length - 5);
        imgPath += $@"Exports\Capture.jpg";

        try
        {
            byte[] img = File.ReadAllBytes(imgPath);
            tex = new Texture2D(Screen.width, Screen.height);
            tex.LoadImage(img);
        }
        catch (System.Exception)
        {
            tex = defaultNull;
        }
        
        tex.Apply();

        GetComponent<Projector>().material.SetTexture("_ShadowTex", tex);
        GameObject.FindGameObjectWithTag("testTag").GetComponent<RawImage>().texture = tex;
    }
}
