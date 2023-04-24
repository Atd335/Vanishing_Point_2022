using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionAudio : MonoBehaviour
{

    public int destroyOnIndex = -1;
    public float fadeSpd;

    float startVolume;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        startVolume = GetComponent<AudioSource>().volume;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == destroyOnIndex)
        {
            fadeAway = true;
        }
    }
    bool fadeAway;

    float timer = 0;
    void Update()
    {
        if (!fadeAway) { return; }
        timer += Time.deltaTime / fadeSpd;
        timer = Mathf.Clamp(timer,0,1);
        GetComponent<AudioSource>().volume = Mathf.Lerp(startVolume,0,timer);
        if (timer == 1) { Destroy(this.gameObject);}
    }
}
