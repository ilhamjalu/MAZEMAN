using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    public Button button;
    public GameManagerScript gmScript;
    public ScoreManager scoreManager;

    public int playerCheck;

    [HideInInspector]
    public int id;
    [Header("Component")]
    public Rigidbody rig;
    public Player photonPlayer;
    public Text playerNickName;
    [SerializeField]
    private float speed = 0.2f;


    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        speed = 0.2f;
        GameManagerScript.instance.players[id - 1] = this;

        if (photonView.IsMine)
        {
            playerCheck = 1;
        }
        else
        {
            playerCheck = 2;
        }

        // if (!photonView.IsMine)
        // {
        //     rig.isKinematic = true;
        // }
    }
    private void Start()
    {
        //rig.isKinematic = true;
        playerNickName.text = photonPlayer.NickName;
        
    }

    private void Update()
    {
        gmScript = GameObject.Find("Manager").GetComponent<GameManagerScript>();
        scoreManager = GameObject.Find("Manager").GetComponent<ScoreManager>();

        //Movements();
        if (photonPlayer.IsLocal)
        {
            Movements();
        }
    }

    void Movements()
    {
        // Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        // controller.Move(move * Time.deltaTime * speed);

        // if (move != Vector3.zero)
        //    transform.forward = move;

        if (gmScript.movHor == 1)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else if(gmScript.movHor == -1)
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
        else if(gmScript.movVert == 1)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        else if(gmScript.movVert == -1)
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Coin")
        {
            //photonPlayer.SetCustomProperties(10);
            //ScoreExtensions.AddScore(photonPlayer, 10);
            //if (photonView.IsMine)
            //{
                photonView.RPC("AddPoint", RpcTarget.All);

            //}
            //scoreManager.textScore.text = "My Score : " + photonPlayer.GetScore().ToString();
            Debug.Log("SCORE");
        }
    }

    [PunRPC]
    void AddPoint(){
        if (playerCheck == 1)
        {
            scoreManager.myScore += 10;
        }
        else if(playerCheck == 2)
        {
            scoreManager.enemyScore += 10;
        }
    }
    
}
