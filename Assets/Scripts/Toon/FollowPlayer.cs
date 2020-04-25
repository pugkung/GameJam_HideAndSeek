using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform PlayerTran;
    public bool stardgo = false;
    public float MoveSpeed = 0.001f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
        if(stardgo)
        transform.position = Vector3.MoveTowards(transform.position, PlayerTran.transform.position, MoveSpeed);
    }
}
