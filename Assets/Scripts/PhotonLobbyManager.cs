using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class PhotonLobbyManager : MonoBehaviourPunCallbacks
{
    const string playerNamePrefKey = "PlayerName";
    const string gameVersion = "1";

    public GameObject DebugText;
    public GameObject JoinRoomButton;
    public GameObject StartGameButton;
    public GameObject PlayerNameInput;

    private Text txtDebug;
    private Button btnJoinRoom;
    private Button btnStartGame;
    private InputField txtPlayerName;

    private bool playerReady;
    private string output;
    private string playerName;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        playerReady = false;

        try {
            btnJoinRoom = JoinRoomButton.GetComponent<Button>();
            btnStartGame = StartGameButton.GetComponent<Button>();
            txtPlayerName = PlayerNameInput.GetComponent<InputField>();

            btnJoinRoom.interactable = false;
            btnStartGame.interactable = false;
            txtPlayerName.interactable = false;
            

            output = "Connecting to server";

            ConnectToPUNServer();
        } catch (System.Exception ex)
        {
            Debug.LogError("Game cannot be iniitalized, please double-check gameobject mapping.");
        }

        if (DebugText != null)
        {
            txtDebug = DebugText.GetComponent<Text>();
        }
    }

    void ConnectToPUNServer()
    {
        if (!PhotonNetwork.IsConnected) {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    void Update()
    {
        if (DebugText != null)
        {
            if (PhotonNetwork.InRoom)
            {
                txtDebug.text = "Current players: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "\n";
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    txtDebug.text += p.NickName + " | ";
                }
            }
            else
            {
                txtDebug.text = output;
            }
        }

        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                btnStartGame.interactable = true;
            }
            else
            {
                btnStartGame.interactable = false;
            }
        } else
        {
            btnStartGame.interactable = false;
        }
    }
        

    public void JoinRoom()
    {
        btnJoinRoom.image.color = Color.yellow;

        if (!PhotonNetwork.InRoom)
        {
            txtPlayerName.interactable = false;
            playerName = txtPlayerName.text;
            PlayerPrefs.SetString(playerNamePrefKey, playerName);
            PhotonNetwork.NickName = playerName;

            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            LeaveRoom();
        }
    }

    public void LeaveRoom()
    {
        txtPlayerName.interactable = true;
        btnJoinRoom.image.color = Color.white;
        PhotonNetwork.LeaveRoom();
    }

    public void PlayerReady()
    {
        // toggle ready status
        playerReady = !playerReady;
        PhotonNetwork.LocalPlayer.CustomProperties["ready"] = true;

        if (playerReady)
        {
            btnStartGame.image.color = Color.green;
            if (isAllPlayersReady())
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                if (PhotonNetwork.IsMasterClient) {
                    PhotonNetwork.LoadLevel("PhotonTest_Game");
                }
            }
        }
        else
        {
            btnStartGame.image.color = Color.white;
            PhotonNetwork.LocalPlayer.CustomProperties["ready"] = false;
        }
    }

    private bool isAllPlayersReady()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object readyVal;
            p.CustomProperties.TryGetValue("ready", out readyVal);
            if (readyVal is bool && (bool) readyVal == false)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// PUN Callbacks, should not be directly invoked
    /// </summary>

    public override void OnConnectedToMaster()
    {
        output = "Connected to PUN server";
        btnJoinRoom.interactable = true;
        txtPlayerName.interactable = true;

        if (PlayerPrefs.HasKey(playerNamePrefKey))
        {
            playerName = PlayerPrefs.GetString(playerNamePrefKey);
            txtPlayerName.text = playerName;
        }
        else
        {
            playerName = "";
        }
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Player disconnected: " + cause.ToString());
        btnJoinRoom.interactable = false;
        btnStartGame.interactable = false;
        txtPlayerName.interactable = false;

        output = "Disconnected from server";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No open room found, creating new room.");
        PhotonNetwork.CreateRoom(null, new RoomOptions());
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
}
