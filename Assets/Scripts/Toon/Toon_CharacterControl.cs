using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toon_CharacterControl : MonoBehaviour
{
    public GameObject PlayerGo;
    public GameObject PlayerGraphic;
    public GameObject PlayerGraphic_walk;
    public GameObject PlayerGraphic_idle;

    public Vector3 PlayerScaleOriginal;
    public float MoveSpeed;

    public GameObject CamGo;
    private Vector3 PlayerMovingDelta = new Vector3(0,0,0);
  
   void Start()
    {
        PlayerScaleOriginal = PlayerGraphic.transform.localScale;
    }

    void Update() {
        float CurrentMoveSpeed = MoveSpeed;
        bool IsWalk = false;
         if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
            CurrentMoveSpeed *= 3;
        }
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
            PlayerGo.transform.position += new Vector3(-CurrentMoveSpeed,0,0);
            PlayerGraphic.transform.localScale = new Vector3(-PlayerScaleOriginal.x,PlayerScaleOriginal.y,PlayerScaleOriginal.z);
            IsWalk = true;
        }
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            PlayerGo.transform.position += new Vector3(CurrentMoveSpeed,0,0);
            PlayerGraphic.transform.localScale = new Vector3(PlayerScaleOriginal.x,PlayerScaleOriginal.y,PlayerScaleOriginal.z);
            IsWalk = true;
        }
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)){
            PlayerGo.transform.position += new Vector3(0,0,CurrentMoveSpeed);
            IsWalk = true;
        }
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)){
            PlayerGo.transform.position += new Vector3(0,0,-CurrentMoveSpeed);
            IsWalk = true;
        }
        if(IsWalk){
            PlayerGraphic_walk.SetActive(true);
            PlayerGraphic_idle.SetActive(false);
        }else{
            PlayerGraphic_walk.SetActive(false);
            PlayerGraphic_idle.SetActive(true);
        }
    }
}
