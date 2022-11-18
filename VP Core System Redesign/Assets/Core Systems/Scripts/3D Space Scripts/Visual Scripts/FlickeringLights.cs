using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLights : MonoBehaviour
{
    public bool isFlickering = false;
    public float timeDelay;
    public float minTimeDelay;
    public float maxTimeDelay;

    // Update is called once per frame
    void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }   
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        this.gameObject.GetComponent<Light>().enabled = false;
        timeDelay = Random.Range(minTimeDelay, maxTimeDelay);
        yield return new WaitForSeconds(timeDelay);
        this.gameObject.GetComponent<Light>().enabled = true;
        timeDelay = Random.Range(minTimeDelay, maxTimeDelay);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
    }

}
