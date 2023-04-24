using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnKeyRelease : MonoBehaviour
{

    public KeyCode key;
    public UnityEvent onRelease;
    public bool onPress;
    void Start()
    {
        
    }

    void Update()
    {
        if (onPress)
        {
            if (Input.GetKeyDown(key))
            {
                onRelease.Invoke();
            }
        }
        else
        {
            if (Input.GetKeyUp(key))
            {
                onRelease.Invoke();
            }
        }
    }
}
