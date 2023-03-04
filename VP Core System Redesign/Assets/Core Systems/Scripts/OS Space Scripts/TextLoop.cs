using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextLoop : MonoBehaviour
{
    int fr;
    public int spd;
    public string[] textArray;
    int index;
    private void FixedUpdate()
    {
        fr++;
        if (fr % spd == 0)
        {
            index += 1;
            if (index == textArray.Length) { index = 0; }

            GetComponent<TextMeshProUGUI>().text = textArray[index];

            fr = 0;
        }
    }
}
