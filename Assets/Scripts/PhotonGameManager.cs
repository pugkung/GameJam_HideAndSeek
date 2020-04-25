using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonGameManager : MonoBehaviourPunCallbacks
{

    public GameObject PlayerPrefab;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        if (PlayerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.Log("Spawning player");
                //Vector3 randomPosition = new Vector3(Random.Range(-40.0f, 4.0f), 1.0f, Random.Range(-21.0f,3.5f));
                Vector3 randomPosition = new Vector3(0, 1.0f, 0);
                PhotonNetwork.Instantiate(this.PlayerPrefab.name, randomPosition, Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }
    
    void Update()
    {
        
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player joined the room");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left the room");
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("PhotonTest_Lobby");
    }
}
