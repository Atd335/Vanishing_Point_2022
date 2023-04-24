using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureHolder : MonoBehaviour
{
    public Texture StoredTex;
    public static TextureHolder th;

    public void Start()
    {
        th = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
    }
}
