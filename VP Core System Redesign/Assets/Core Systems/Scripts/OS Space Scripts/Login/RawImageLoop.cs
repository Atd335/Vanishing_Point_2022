using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImageLoop : MonoBehaviour
{
    public Vector2 scrollSpeed;
    Vector2 pos;
    RawImage img;

    private void Start()
    {
        img = GetComponent<RawImage>();
    }
    void Update()
    {
        pos += scrollSpeed * Time.deltaTime;
        img.uvRect = new Rect(pos.x, pos.y, img.uvRect.width, img.uvRect.height);
    }
}
