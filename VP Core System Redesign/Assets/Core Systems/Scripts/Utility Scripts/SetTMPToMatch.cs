using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetTMPToMatch : MonoBehaviour
{
    public TextMeshProUGUI textToCopy;
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = textToCopy.text;
    }
}
