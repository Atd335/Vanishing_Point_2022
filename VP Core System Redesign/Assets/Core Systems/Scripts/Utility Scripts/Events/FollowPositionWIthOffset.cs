using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPositionWIthOffset : MonoBehaviour
{

    public Transform transformToFollow;
    public float ForwardOffset;

    private void Start()
    {
        transform.parent = transformToFollow.parent;
        transform.localPosition = new Vector3(0,0,ForwardOffset);
    }

    void LateUpdate()
    {
        
    }
}
