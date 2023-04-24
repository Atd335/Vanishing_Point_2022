using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginDiffuse : MonoBehaviour
{
    public ReactionDiffusionTexture rdt;
    RawImage img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<RawImage>();
        img.texture = rdt.displayTexture;
        rdt.clearRawTexture();
        rdt.calculateRawTexture();
        rdt.stampImage(.5f,.5f,.4f);
    }
    public void begin(bool _b)
    {
        b = _b;
    }
    bool b;
    // Update is called once per frame
    void Update()
    {
        if (!b) { return; }
        rdt.calculateRawTexture();
    }
}
