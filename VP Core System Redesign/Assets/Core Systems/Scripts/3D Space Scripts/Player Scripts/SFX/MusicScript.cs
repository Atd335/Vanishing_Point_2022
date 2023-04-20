using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{

    public AudioSource AS;
    public bool playOnStart;

    public AudioClip song;
    public float volume = 1;
	public void _Start()
    {
        AS = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        AS.loop = true;
        if (playOnStart)
        {
            AS.clip = song;
            AS.Play();
        }
    }

    public void _Update()
    {
        AS.volume = volume;
    }
}
