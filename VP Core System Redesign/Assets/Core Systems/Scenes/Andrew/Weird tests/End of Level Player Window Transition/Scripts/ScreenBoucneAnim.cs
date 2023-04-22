using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBoucneAnim : MonoBehaviour
{

    float timer;
    public float spd;
    public float duration;
    public float magnitude;

    public float xMul = 1;
    public float yMul = 1;

    public Vector3 initScale;
    float initScaleTimer;
    public float initScaleTimerDuration = .2f;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = initScale;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime/duration;
        timer = Mathf.Clamp(timer,0,1);

        float xSpring = (Mathf.Sin(timer*spd)*Mathf.Lerp(magnitude,0,timer)) * xMul;
        float ySpring = -(Mathf.Sin(timer*spd)*Mathf.Lerp(magnitude,0,timer)) * yMul;

        initScaleTimer += Time.deltaTime / initScaleTimerDuration;
        initScaleTimer = Mathf.Clamp(initScaleTimer,0,1);

        transform.localScale =  Vector3.Lerp(initScale,new Vector3(1+xSpring,1+ySpring,1),initScaleTimer);
    }
}
