using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC3D_BareBones : MonoBehaviour
{
    //tuning variables
    public float mouseSensitivity = 1;
    public float playerSpd = 6;
    public float jumpHeight = 1.3f;
    public float gravity = 6;
    public float bobTimer;
    public float bobSpd = 10f;
    public float bobMag = .05f;

    Vector3 moveDirection;
    Vector3 cameraRotation;
    Vector3 headPos;

    public AudioClip[] footSteps;

    public GameObject player3D;

    CharacterController CC;

    public Transform head;
    public Transform headmast;

    public bool blockMovement;

    public void _Start()
    {
        CC = player3D.GetComponent<CharacterController>();
        head = GameObject.FindWithTag("Head 3D").transform;
        headmast = GameObject.FindWithTag("Head Mast 3D").transform;
        cameraRotation = head.localRotation.eulerAngles;
        headPos = head.localPosition;
    }
    public void _Update()
    {
        if (blockMovement) { return; }

        Vector3 v = new Vector3(moveDirection.x, 0, moveDirection.z);

        if (v.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * bobSpd;
            head.localPosition = headPos + new Vector3(0, Mathf.Sin(bobTimer) * bobMag, 0);
        }
        else
        {
            bobTimer = 0;
            head.localPosition = Vector3.Lerp(head.localPosition, headPos, Time.deltaTime * 12f);
        }

        cameraRotation += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * mouseSensitivity;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90, 90);
        head.rotation = Quaternion.Euler(cameraRotation);

        headmast.rotation = Quaternion.Euler(0, cameraRotation.y, 0);

        moveDirection = new Vector3(Input.GetAxis("Horizontal_3D"), moveDirection.y, Input.GetAxis("Vertical_3D"));
        moveDirection = moveDirection.x * headmast.right +
                        moveDirection.z * headmast.forward +
                        moveDirection.y * headmast.up;

        if (CC.isGrounded)
        {
            moveDirection.y = 0;
            if (Input.GetButton("Jump_3D"))
            {
                moveDirection.y = jumpHeight;
            }

            playFootstepSounds();
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        CC.Move(moveDirection * playerSpd * Time.deltaTime);
    }

    float footStepTimer = 0;
    float stepTime = .4f;
    void playFootstepSounds()
    {
        //play footsteps on movement
        if (new Vector2(moveDirection.x, moveDirection.z).magnitude > 0.1f)// if the player is moving and on the ground
        {
            footStepTimer += Time.deltaTime;
            if (footStepTimer >= stepTime)
            {
                GetComponent<SFXScript>().playSFX(footSteps[Random.Range(0,footSteps.Length)]);
                footStepTimer = 0;
            }
        }
    }
}
