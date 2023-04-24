using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDiffuseReactionIntro : MonoBehaviour
{   

    public ReactionDiffusionTexture rdt;
    public ApplyDiffusionReactionToCamera camTex;
    public NoiseJitter noise;
    // Start is called before the first frame update
    public int calculationMultiplier = 20;
    public void Begin()
    {
        rdt.clearRawTexture();
        camTex.enabled = true;
        trigger = true;       
        StartCoroutine(stampRoutine());
    }
    IEnumerator stampRoutine()
    {
        rdt.stampImage(.2f,.2f,.2f);
        yield return new WaitForSeconds(.1f);
        rdt.stampImage(.2f,.8f,.2f);
        yield return new WaitForSeconds(.1f);
        rdt.stampImage(.8f,.8f,.2f);
        yield return new WaitForSeconds(.1f);
        rdt.stampImage(.8f,.2f,.2f);
        yield return new WaitForSeconds(.1f);
        rdt.stampImage(.5f, .5f, .6f);

    }

    public bool trigger = false;
    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        if (!trigger) { return; }
        rdt.calculateRawTexture(calculationMultiplier);
        print("tick");
        timer += Time.deltaTime / 2;
        noise.noiseStrength = Mathf.Lerp(.1f,.7f,timer);
    }
}
