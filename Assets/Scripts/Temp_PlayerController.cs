using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temp_PlayerController : MonoBehaviourPunCallbacks
{
    private PlayerManager target;
    private CameraWork playerCam;
    private Text playerNameText;
    private int playerID;
    private float speed = 5.0f;


    void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;

            // setup camera on 'my' controllable character only
            playerCam = gameObject.AddComponent<CameraWork>();
            playerCam.OnStartFollowing();

        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
    }

    public void SetTarget(PlayerManager _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }

        // Cache references for efficiency
        target = _target;
        if (playerNameText != null)
        {
            playerNameText.text = target.photonView.Owner.NickName;
            playerID = target.photonView.Owner.ActorNumber;
        }
    }
}
