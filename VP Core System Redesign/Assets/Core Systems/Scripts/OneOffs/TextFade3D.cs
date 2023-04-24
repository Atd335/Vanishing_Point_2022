using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFade3D : MonoBehaviour
{
    TextMeshPro text;
    public Color startColor;
    public Color endColor;

    public bool startWithEndColor;

    public void fadeToStartColor(float dur)
    {
        StartCoroutine(fadeCoRoutine(dur, 0));   
    }

    public void fadeToEndColor(float dur)
    {
        StartCoroutine(fadeCoRoutine(dur, 1));
    }

    bool fading;
    IEnumerator fadeCoRoutine(float dur, int dir)
    {
        if (!fading)
        {
            fading = true;
            float timer = 1 - dir;

            while (timer != dir)
            {
                text.color = Color.Lerp(startColor, endColor, timer);
                timer += (Time.deltaTime / dur) * ((dir * 2) - 1);
                timer = Mathf.Clamp(timer, 0, 1);
                yield return new WaitForEndOfFrame();
            }
            fading = false;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        if (startWithEndColor) { text.color = endColor; }
    }

}
