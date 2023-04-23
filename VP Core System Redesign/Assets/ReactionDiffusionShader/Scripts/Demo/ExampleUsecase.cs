using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleUsecase : MonoBehaviour
{
    public ReactionDiffusionTexture rdt;
    public ApplyDiffusionReactionToCamera camTex;
    public NoiseJitter noise;

    // Start is called before the first frame update
    void Start()
    {
        rdt.clearRawTexture();
        UpdateDriver.ud.GetComponent<ModeSwitcher>().onFPS.AddListener(resetTex);
        UpdateDriver.ud.GetComponent<ModeSwitcher>().on2D.AddListener(triggerPattern);
    }



    void triggerPattern()
    {
        StartCoroutine(LoopPattern(60,.2f,1));
    }

    IEnumerator LoopPattern(int iter, float delay,float fadeDur)
    {
        while (true)
        {
            StartCoroutine(iteratePattern(iter, delay));
            yield return new WaitForSeconds((iter*delay)/2);
            StartCoroutine(fadeTexAway(fadeDur));
            yield return new WaitForSeconds(fadeDur+.5f);
        }
    }

    IEnumerator fadeTexAway(float dur)
    {
        float timer = 0;

        while (timer!=1)
        {
            timer += Time.deltaTime / dur;
            timer = Mathf.Clamp(timer,0,1);
            noise.noiseStrength = Mathf.Lerp(.6f,0,timer);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator iteratePattern(int iter, float delay)
    {
            rdt.clearRawTexture();
            camTex.intensity = 1;
            noise.noiseStrength = .6f;
            for (int i = 0; i < iter; i++)
            {
                yield return new WaitForSeconds(delay);
                float x = Random.Range(.05f, .95f);
                float y = Random.Range(.05f, .95f);
                float s = Random.Range(.05f, .3f);
                rdt.stampImage(x, y, s);
            }
    }

    void resetTex()
    {
        rdt.clearRawTexture();
        StopAllCoroutines();
    }

    void Update()
    {
        bool fps = UpdateDriver.ud.GetComponent<ModeSwitcher>().firstPerson;
        camTex.enabled = !fps;
        if (fps) { return; }
        rdt.calculateRawTexture();       
    }
}
