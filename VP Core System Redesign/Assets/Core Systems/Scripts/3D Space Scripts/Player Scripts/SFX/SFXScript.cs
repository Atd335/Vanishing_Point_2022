using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXScript : MonoBehaviour
{

    public AudioSource AS;
    public AudioClip[] clips;

	public void _Start()
    {
        AS = this.gameObject.AddComponent<AudioSource>() as AudioSource;

        UpdateDriver.ud.GetComponent<ModeSwitcher>().on2D.AddListener(transition); 
        UpdateDriver.ud.GetComponent<ModeSwitcher>().onFPS.AddListener(transition); 

    }

    public void playSFX(AudioClip clip, float volume = .1f)
    {
        AS.PlayOneShot(clip, volume);
    }


    void transition()
    {
        print("HELLO");
        if (UpdateDriver.ud.GetComponent<ModeSwitcher>().firstPerson)
        {
            playSFX(clips[0]);
        }
        else
        {
            playSFX(clips[1]);
        }

    }
}
