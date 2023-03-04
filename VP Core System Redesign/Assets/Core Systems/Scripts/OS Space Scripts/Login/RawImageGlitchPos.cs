using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImageGlitchPos : MonoBehaviour
{
    public Vector2 intervalRange;
    public Vector2 posRange;
    Vector2 pos;
    RawImage img;

    private void Start()
    {
        img = GetComponent<RawImage>();
        spd = Random.Range(intervalRange.x, intervalRange.y);
    }

    float timer = 0;
    float spd;
    void Update()
    {
        timer += Time.deltaTime * spd;
        timer = Mathf.Clamp(timer,0,1);
        if (timer >= 1)
        {
            pos += new Vector2(Random.Range(posRange.x,posRange.y), Random.Range(posRange.x, posRange.y));
            timer = 0;
            spd = Random.Range(intervalRange.x, intervalRange.y);
        }

        img.uvRect = new Rect(pos.x, pos.y, img.uvRect.width, img.uvRect.height);
    }
}
