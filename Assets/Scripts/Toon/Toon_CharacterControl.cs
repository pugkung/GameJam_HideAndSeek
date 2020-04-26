using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Toon_CharacterControl : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback

{
    public GameObject PlayerGo;
    public GameObject PlayerGraphic;
    public GameObject PlayerGraphic_walk;
    public GameObject PlayerGraphic_idle;
    public GameObject Player_SpotLight;
    public GameObject PlayerNameUI;

    public Vector3 PlayerScaleOriginal;
    public float MoveSpeed;
    public int id;
    public bool player = false;

    public GameObject[] AnimWalk;
    public GameObject[] AnimIdle;

    public GameObject CamGo;
    //private CameraWork playerCam;
    private Vector3 PlayerMovingDelta = new Vector3(0,0,0);

    void Awake()
    {
        RandomAssignPlayerRole();
        PlayerGraphic_(photonView.CreatorActorNr%3);
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
            player = true;
            // show player name
            PlayerNameUI.GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
       
            // check my role
            object myRole;
            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("role", out myRole);
            if (myRole.ToString() == "seeker")
            {
                Player_SpotLight.SetActive(true);
            }
            else
            {
                PlayerGo.tag = "OtherPlayer";
            }

            // setup camera on 'my' controllable character only
            CamGo = GameObject.Find("Main Camera");

            TransformFollower tf = CamGo.GetComponent<TransformFollower>();
            tf.enabled = true;
            tf.target = this.gameObject.transform;
        }
        else
        {
           
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void PlayerGraphic_(int i)
    {
        id = i;
        PlayerGraphic_walk = AnimWalk[id];
        PlayerGraphic_idle = AnimIdle[id];
    }

    void Start()
    {
          PlayerScaleOriginal = PlayerGraphic.transform.localScale;
    }

    void Update() {
        bool IsWalk = false;
        if (player)
        {
            float CurrentMoveSpeed = MoveSpeed;
           
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                CurrentMoveSpeed *= 3;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                PlayerGo.transform.position += new Vector3(-CurrentMoveSpeed, 0, 0);
                PlayerGraphic.transform.localScale = new Vector3(-PlayerScaleOriginal.x, PlayerScaleOriginal.y, PlayerScaleOriginal.z);
                IsWalk = true;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                PlayerGo.transform.position += new Vector3(CurrentMoveSpeed, 0, 0);
                PlayerGraphic.transform.localScale = new Vector3(PlayerScaleOriginal.x, PlayerScaleOriginal.y, PlayerScaleOriginal.z);
                IsWalk = true;
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                PlayerGo.transform.position += new Vector3(0, 0, CurrentMoveSpeed);
                IsWalk = true;
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                PlayerGo.transform.position += new Vector3(0, 0, -CurrentMoveSpeed);
                IsWalk = true;
            }
           
        }
        if (IsWalk)
        {
            PlayerGraphic_walk.SetActive(true);
            PlayerGraphic_idle.SetActive(false);
        }
        else
        {
            PlayerGraphic_walk.SetActive(false);
            PlayerGraphic_idle.SetActive(true);
        }
    }

    void RandomAssignPlayerRole()
    {
        int totalPlayers = PhotonNetwork.PlayerList.Length;
        int seekerPlayer = Random.Range(0, totalPlayers);

        for (int i=0; i< totalPlayers; i++)
        {
            if (i == seekerPlayer)
            {
                PhotonNetwork.PlayerList[i].CustomProperties["role"] = "seeker";
            }
            else
            {
                PhotonNetwork.PlayerList[i].CustomProperties["role"] = "hider";
            }
        }
    }

    public void OnPhotonInstantiate(Photon.Pun.PhotonMessageInfo info)
    {
        // Hide everyone beside me
        if (!info.Sender.IsLocal)
        {
            Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in allRenderers)
            {
                renderer.enabled = false;
            }

            Light[] allPlayerLights = gameObject.GetComponentsInChildren<Light>();
            foreach (Light light in allPlayerLights)
            {
                light.enabled = false;
            }

            Canvas[] allPlayerUI = gameObject.GetComponentsInChildren<Canvas>();
            foreach (Canvas ui in allPlayerUI)
            {
                ui.enabled = false;
            }
        }
    }
}
