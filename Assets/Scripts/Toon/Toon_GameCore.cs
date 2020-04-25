using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toon_GameCore : MonoBehaviour
{
    public GameObject goPlayer;
    private BoxCollider collAreaPlayer;

    public GameObject RedScreen;

    void Start()
    {
        collAreaPlayer = goPlayer.gameObject.GetComponent<BoxCollider>();
        
    }

    // Update is called once per frame
    void Update(){
        //string sRayCastObjectFound = "";
        /*GameObject goHit = null;
		Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
		RaycastHit hit;
		Physics.Raycast (ray,out hit,nEventObjectLength);
		if (hit.collider != null) {
			if (hit.collider.gameObject.tag == "Item") {
				//sRayCastObjectFound = hit.collider.gameObject.name;
                goHit = hit.collider.gameObject;
                Debug.Log(hit.collider.gameObject.name);
			} 
		}*/
        GameObject[] AllItems = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject Item in AllItems){
           if (collAreaPlayer.bounds.Contains (Item.transform.position) && Item.activeInHierarchy) {
               //Item.SetActive(false);
               Destroy(Item);
           }
        }

        GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("OtherPlayer");
        foreach (GameObject Player in AllPlayers){
           if (collAreaPlayer.bounds.Contains (Player.transform.position) && Player != null) {
               //Item.SetActive(false);
               Destroy(Player);
               //GoHit = Player;
               //break;
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
    }
}
