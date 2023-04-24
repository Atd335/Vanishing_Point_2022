using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedEvent : MonoBehaviour
{
    public float time;
    public UnityEvent timedEvent;
    void Start()
    {
        StartCoroutine(throwEvent());
    }

    IEnumerator throwEvent()
    {
        yield return new WaitForSeconds(time);
        timedEvent.Invoke();
    }

    public void playSound(AudioClip clip)
    {
        if (!GetComponent<AudioListener>())
        {
            this.gameObject.AddComponent<AudioListener>();
        }


        if (!GetComponent<AudioSource>())
        {
            this.gameObject.AddComponent<AudioSource>();
        }

        GetComponent<AudioSource>().PlayOneShot(clip,transform.localScale.z);
        

    }

}
