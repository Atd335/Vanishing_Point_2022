using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadBar : MonoBehaviour
{   

    public AnimationCurve loadingBar;
    public bool b;
    public float dur;
    public RectTransform loadBar;

    public UnityEvent onLoaded;
    public UnityEvent onLoadedSuccess;

    public static Vector2 loadRange;
    public static int sceneIndex;
    private void Awake()
    {
        b = true;
        if (loadRange == Vector2.zero) { loadRange = new Vector2(0, 1); }
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneLoaderEasy.sceneToLoad = sceneIndex;
    }

    public void begin()
    {
        b = true;
    }

    float timer;

    // Update is called once per frame
    void Update()
    {
        if (!b) { return; }
        timer += Time.deltaTime / dur;
        timer = Mathf.Clamp(timer,0,1);

        loadBar.localScale =new Vector3(Mathf.Lerp(loadRange.x, loadRange.y, loadingBar.Evaluate(timer)), 1,1);
        if (timer == 1) 
        {
            if (loadRange.y == 1) { 
                onLoadedSuccess.Invoke(); 
                
                this.enabled = false; 
                return; }
            onLoaded.Invoke(); this.enabled = false; 
        }
    }
}
