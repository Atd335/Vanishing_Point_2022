using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using UnityEngine.UI;

public class WelcomeScreenVideoContainer : MonoBehaviour
{   
    void Start()
    {
        VP = GetComponentInChildren<VideoPlayer>();
    }
    float timer = 0;
    float videoTimer = 0;
    public UnityEvent onVideoFinish;
    public float VideoDurationInSeconds;
    VideoPlayer VP;

    public float fadeTime = 3;

    void Update()
    {
        timer += Time.deltaTime / fadeTime;
        timer = Mathf.Clamp(timer,0,1);
        GetComponentInChildren<RawImage>().color = Color.Lerp(Color.clear, Color.white, timer);


        videoTimer += Time.deltaTime;
        videoTimer = Mathf.Clamp(videoTimer,0,VideoDurationInSeconds);
        if (videoTimer >= VideoDurationInSeconds)
        {
            onVideoFinish.Invoke();
            this.enabled = false;
        }
    }
}
