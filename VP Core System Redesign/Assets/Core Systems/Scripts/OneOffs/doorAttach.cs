//script called "doorAttach"
//when the player enters the trigger, the bool activating the door opening animation flips to true, and on leaving it returns to false

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorAttach : MonoBehaviour
{
    public GameObject Player;

    private Animator doorAnim;

    void Start()
    {
        doorAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            doorAnim.SetBool("Open", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            doorAnim.SetBool("Open", false);
        }
    }
}