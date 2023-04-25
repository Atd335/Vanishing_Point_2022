//script called "elevatorAttach"
//attached to elevator object, using a trigger, the player can trigger the elevator to lift and get carried with it by entering said trigger

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevatorAttach : MonoBehaviour
{

    public GameObject Player;
    private Animator elevatorAnim;


    void Start()
    {
        elevatorAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = transform;
            elevatorAnim.SetBool("Lifting", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;
            elevatorAnim.SetBool("Lifting", false);
        }
    }

}