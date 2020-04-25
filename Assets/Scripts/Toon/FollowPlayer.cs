using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform PlayerTran;
    public float MoveSpeed = 0.001f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
        //transform.position = Vector3.MoveTowards(transform.position, PlayerTran.transform.position, MoveSpeed);
    }
}
