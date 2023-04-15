using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIOSTransfer : MonoBehaviour
{
    public GameObject[] objects;
    public void activate()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(true);
        }
    }

}
