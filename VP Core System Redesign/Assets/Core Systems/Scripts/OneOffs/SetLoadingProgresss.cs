using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLoadingProgresss : MonoBehaviour
{
    public Vector2 loadingBar;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        LoadBar.loadRange = loadingBar;
        LoadBar.sceneIndex = index;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
