using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginEvents : MonoBehaviour
{

    Vector2 lerpPos;
    RectTransform rt;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        lerpPos = rt.anchoredPosition;
    }
    public void changePos(string str)
    {
        Vector3 v;

        v.x = float.Parse(str.Split(',')[0]);
        v.y = float.Parse(str.Split(',')[1]);
        v.z = float.Parse(str.Split(',')[2]);

        lerpPos = v;
    }
    // Update is called once per frame
    void Update()
    {
        rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition,lerpPos,Time.deltaTime * 5);
    }
}
