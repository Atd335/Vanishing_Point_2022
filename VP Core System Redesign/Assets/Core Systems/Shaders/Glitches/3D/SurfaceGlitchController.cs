using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceGlitchController : MonoBehaviour
{
    MeshRenderer mr;
    Material mat;
    public Shader surfaceGlitch;

    public float glitchMagnitude = .5f;
    public float offsetPosMagnitude = .1f;
    public float offsetScaleMagnitude = .1f;

    public Vector2 rateVariance = new Vector2(0.1f,1f);

    public bool playSound;
    public AudioClip clip;
    AudioSource AS;
    public float volume = .5f;
    public float soundRadius = 3;
    public float GlitchDur = .1f;

    public float rotateChance = 0f;
    Quaternion initRot;
    void Start()
    {
        if (GetComponent<MeshRenderer>() == null) { this.enabled = false; return; }
        mr = GetComponent<MeshRenderer>();
        mat = new Material(surfaceGlitch);
        rate = Random.Range(rateVariance.x,rateVariance.y);

        mr.material = mat;

        if (playSound)
        {
            this.gameObject.AddComponent<AudioSource>();
            AS = GetComponent<AudioSource>();
            AS.spatialBlend = 1;
            AS.maxDistance = soundRadius;
            AS.minDistance = soundRadius;
        }

        initRot = transform.rotation;
    }

    void Glitch(float dur)
    {
        StartCoroutine(glitchMesh(dur));   
    }

    IEnumerator glitchMesh(float dur)
    {
        float r = Random.Range(0,1f);
        if (r <= rotateChance)
        {
            transform.Rotate(
                Random.Range(-360, 360f),
                Random.Range(-360, 360f),
                Random.Range(-360, 360f)
                );
        }
        else { transform.rotation = initRot; }

        float g = Random.Range(-glitchMagnitude,glitchMagnitude);
        Vector3 pos = new Vector3(
            Random.Range(-offsetPosMagnitude,offsetPosMagnitude),
            Random.Range(-offsetPosMagnitude,offsetPosMagnitude),
            Random.Range(-offsetPosMagnitude,offsetPosMagnitude)
            );
        
        Vector3 scl = new Vector3(
            Random.Range(-offsetScaleMagnitude,offsetScaleMagnitude),
            Random.Range(-offsetScaleMagnitude,offsetScaleMagnitude),
            Random.Range(-offsetScaleMagnitude,offsetScaleMagnitude)
            );

        mat.SetFloat("_Amount", g);
        mat.SetVector("_OffsetPos", pos);
        mat.SetVector("_OffsetScale", scl + Vector3.one);

        yield return new WaitForSeconds(dur);
        mat.SetFloat("_Amount", 0);
        mat.SetVector("_OffsetPos", Vector3.zero);
        mat.SetVector("_OffsetScale", Vector3.one);
    }

    float timer = 0;
    float rate;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timer = Mathf.Clamp(timer, 0, rate);
        if (timer == rate)
        {
            try
            {
                AS.PlayOneShot(clip, volume);
            }
            catch (System.Exception){}

            Glitch(GlitchDur);
            rate = Random.Range(rateVariance.x, rateVariance.y);
            timer = 0;
        }
    }

    //mat.SetFloat("_Amount",g);
    //mat.SetVector("_OffsetPos",p);
    //mat.SetVector("_OffsetScale",s);
}
