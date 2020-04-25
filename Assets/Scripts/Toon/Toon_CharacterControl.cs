using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
 
public class Toon_CharacterControl : MonoBehaviourPunCallbacks
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

    public GameObject[] AnimWalk;
    public GameObject[] AnimIdle;

    public GameObject CamGo;
    //private CameraWork playerCam;
    private Vector3 PlayerMovingDelta = new Vector3(0,0,0);

    void Awake()
    {
        RandomAssignPlayerRole();

        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;

            // show player name
            PlayerNameUI.GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
            id = PhotonNetwork.LocalPlayer.ActorNumber;
            PlayerGraphic_walk = AnimWalk[id ];
            PlayerGraphic_idle = AnimIdle[id ];
            // check my role
            object myRole;
            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("role", out myRole);
            if (myRole.ToString() == "seeker")
            {
                Player_SpotLight.SetActive(true);
            }

            // setup camera on 'my' controllable character only
            CamGo = GameObject.Find("Main Camera");

            // I should not see anyone else
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if(PhotonNetwork.LocalPlayer != player)
                {
                    // TODO
                }
            }

            TransformFollower tf = CamGo.GetComponent<TransformFollower>();
            tf.enabled = true;
            tf.target = this.gameObject.transform;
        }
        DontDestroyOnLoad(this.gameObject);
    }

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

    void RandomAssignPlayerRole()
    {
        int totalPlayers = PhotonNetwork.PlayerList.Length;
        int seekerPlayer = Random.Range(-1, totalPlayers) + 1;

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
}
