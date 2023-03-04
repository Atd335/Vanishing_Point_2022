using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageGlitchScript : MonoBehaviour
{

    int fr;
    int spd = 1;
    public Sprite[] spriteArray;
    public Image img;
    private void Start()
    {
        spd = Random.Range(1,10);
    }

    private void FixedUpdate()
    {
        fr++;
        if (fr % spd == 0)
        {
            img.sprite = spriteArray[Random.Range(0,spriteArray.Length)];
            spd = Random.Range(1, 10);
            fr = 0;
        }
    }
}
