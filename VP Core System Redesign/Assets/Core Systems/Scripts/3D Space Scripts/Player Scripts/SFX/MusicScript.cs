using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{

    public AudioSource AS;
    public float volumeMod = 1;
	public void _Start()
    {
        AS = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        AS.loop = true;
        if (UpdateDriver.ud.song != null)
        {
            AS.clip = UpdateDriver.ud.song;
            AS.Play();
        }

    }

    public void _Update()
    {

    }
}
