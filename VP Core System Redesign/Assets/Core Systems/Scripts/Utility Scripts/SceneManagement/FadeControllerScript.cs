using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeControllerScript : MonoBehaviour
{

    public static FadeControllerScript fade;

    public Image fadeIMG;
    public RawImage fadeIMG_Raw;

    public Color transparentColor = new Color(0,0,0,0);
    public Color opaqueColor = new Color(0,0,0,1);

    public UnityEvent onOpaque;
    public UnityEvent onTransparent;

    public bool startTransparent = true;
    
    private void Awake()
    {
        fade = this;

        fadeIMG = GetComponent<Image>();
        if (fadeIMG && startTransparent) { fadeIMG.color = transparentColor; }

        fadeIMG_Raw = GetComponent<RawImage>();
        if (fadeIMG_Raw && startTransparent) {fadeIMG_Raw.color = transparentColor; }
        

        //onOpaque = new UnityEvent();
        //onTransparent = new UnityEvent();
    }

    public void setImageOpaque()
    {
        if (fadeIMG) { fadeIMG.color = opaqueColor; }
        else {fadeIMG_Raw.color = opaqueColor;  }
          
    }

    public void setImageTransparent()
    {
        if (fadeIMG) {fadeIMG.color = transparentColor; }
        else { fadeIMG_Raw.color = transparentColor;}
        
    }

    public void setTransparentColor(Color color)
    {
        transparentColor = color;
    }

    public void setOpaqueColor(Color color)
    {
        opaqueColor = color;
    }

    public void setOpaqueColor(string colorString)
    {
        Color color;
        color.r = float.Parse(colorString.Split(',')[0].ToString());
        color.g = float.Parse(colorString.Split(',')[1].ToString());
        color.b = float.Parse(colorString.Split(',')[2].ToString());
        color.a = float.Parse(colorString.Split(',')[3].ToString());

        opaqueColor = color;
    }

    public void setTransparentColor(string colorString)
    {
        Color color;
        color.r = float.Parse(colorString.Split(',')[0].ToString());
        color.g = float.Parse(colorString.Split(',')[1].ToString());
        color.b = float.Parse(colorString.Split(',')[2].ToString());
        color.a = float.Parse(colorString.Split(',')[3].ToString());

        transparentColor = color;
    }

    public void fadeToOpaque(float speed = .1f)
    {
        StartCoroutine(colorFade(transparentColor,opaqueColor,speed));
    }

    public void fadeToTransparent(float speed = .1f)
    {
        StartCoroutine(colorFade(opaqueColor, transparentColor, speed));
    }

    IEnumerator colorFade(Color col1, Color col2, float spd)
    {
        
        float timer = 0;
        while (timer!=1)
        {
            timer += Time.deltaTime / spd;
            timer = Mathf.Clamp(timer,0,1);
            if (fadeIMG) {fadeIMG.color = Color.Lerp(col1,col2,timer); }
            else { fadeIMG_Raw.color = Color.Lerp(col1, col2, timer); }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (col2 == transparentColor) { onTransparent.Invoke(); }
        else{ onOpaque.Invoke(); }
    }
}
