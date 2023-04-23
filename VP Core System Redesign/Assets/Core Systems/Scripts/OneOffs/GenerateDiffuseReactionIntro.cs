using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDiffuseReactionIntro : MonoBehaviour
{   

    public ReactionDiffusionTexture rdt;
    public ApplyDiffusionReactionToCamera camTex;
    public NoiseJitter noise;
    // Start is called before the first frame update
    public void Begin()
    {
        rdt.clearRawTexture();
        camTex.enabled = true;
        trigger = true;
        rdt.stampImage(.5f,.5f,.6f);
        print("AHHHH");

        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(.05f, .95f);
            float y = Random.Range(.05f, .95f);
            float s = Random.Range(.05f, .3f);
            rdt.stampImage(x, y, s);
        }
    }
    public bool trigger = false;
    // Update is called once per frame
    void Update()
    {
        if (!trigger) { return; }
        rdt.calculateRawTexture(30);
        print("tick");
    }
}
