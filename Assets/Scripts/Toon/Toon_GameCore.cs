using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toon_GameCore : MonoBehaviour
{
    public GameObject goPlayer;
    private BoxCollider collAreaPlayer;

    public GameObject RedScreen;
    public GameObject TextShake;

    public GameObject[] AllTrees;

    void Start()
    {
        collAreaPlayer = goPlayer.gameObject.GetComponent<BoxCollider>();
        AllTrees = GameObject.FindGameObjectsWithTag("Tree");
    }

    // Update is called once per frame
    void Update(){
        GameObject[] AllItems = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject Item in AllItems){
           if (collAreaPlayer.bounds.Contains (Item.transform.position) && Item.activeInHierarchy) {
               Destroy(Item);
           }
        }

        GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("OtherPlayer");
        foreach (GameObject Player in AllPlayers){
           if (collAreaPlayer.bounds.Contains (Player.transform.position) && Player != null) {
               Destroy(Player);
           }
        }
        bool IsHitByEnemy = false;
        GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject Enemy in AllEnemies){
           if (collAreaPlayer.bounds.Contains (Enemy.transform.position) && Enemy != null) {
               IsHitByEnemy = true;
           }
        }
        if(IsHitByEnemy){
            RedScreen.SetActive(true);
        }else{
            RedScreen.SetActive(false);
        }

        bool IsInTreeArea = false;
        foreach (GameObject Tree in AllTrees){
           if (Tree.GetComponent<CapsuleCollider>().bounds.Contains (collAreaPlayer.transform.position) && Tree != null) {
               IsInTreeArea = true;
               if(Input.GetKey(KeyCode.Space)){
                    Tree.SetActive(false);
                    Tree.SetActive(true);
                    Debug.Log(555);
                }
            }
        }
         if(IsInTreeArea){
            TextShake.SetActive(true);
        }else{
            TextShake.SetActive(false);
        }
    }
}
