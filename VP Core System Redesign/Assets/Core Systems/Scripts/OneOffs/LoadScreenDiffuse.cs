using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreenDiffuse : MonoBehaviour
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
        StartCoroutine(loadLoop());
    }

    IEnumerator loadLoop()
    {
        rdt.clearRawTexture();
        rdt.calculateRawTexture();
        rdt.stampImage(.5f, .5f, .4f);
        img.color = new Color(1,1,1,.05f);
        StartCoroutine(fadeTex(img.color.a, 4));
        yield return null;
    }

    IEnumerator fadeTex(float opacity, float dur)
    {
        float timer = 0;
        while (timer!=1)
        {
            timer += Time.deltaTime / dur;
            timer = Mathf.Clamp(timer,0,1);

            img.color = new Color(1,1,1,Mathf.Lerp(opacity,0,timer));
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(loadLoop());
    }

    public void begin(bool _b)
    {
        b = _b;
    }
    public bool b;
    // Update is called once per frame
    void Update()
    {
        if (!b) { return; }
        rdt.calculateRawTexture();
    }
}
