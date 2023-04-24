using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButtonBehavior : MonoBehaviour
{
    [SerializeField] bool inButtonArea;
    [SerializeField] Animator animator;
    [SerializeField] string DoorOpenAnimationName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inButtonArea) 
            {
                Debug.Log("elevator Opening Event");
                animator.Play(DoorOpenAnimationName);
            }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ass entered");
        inButtonArea = true;

    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("ass exited");
        inButtonArea = false;
    }
}
