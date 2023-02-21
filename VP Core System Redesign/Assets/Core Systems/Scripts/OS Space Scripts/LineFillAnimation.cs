using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LineFillAnimation : MonoBehaviour
{

    int fr = 0;
    public int spd = 6;

    Image[] lines;
    Vector2 size;

    public UnityEvent onFilled;

    private void Start()
    {
        lines = GetComponentsInChildren<Image>();
        size = new Vector2(0,100);
    }

    private void FixedUpdate()
    {
        fr++;
        if (fr % spd == 0)
        {
            size.x += 100;
            foreach (Image line in lines)
            {
                line.rectTransform.sizeDelta = size;
            }
            fr = 0;
        }

        if (size.x > Screen.width) { onFilled.Invoke(); Destroy(this); }
    }

}
