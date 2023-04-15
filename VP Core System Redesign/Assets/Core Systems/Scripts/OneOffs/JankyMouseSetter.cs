using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JankyMouseSetter : MonoBehaviour
{   
	public void _Start()
    {
        
    }
    public void _Update()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //I HATE THIS STUPID THING
        Cursor.lockState = CursorLockMode.Confined;
    }
}
