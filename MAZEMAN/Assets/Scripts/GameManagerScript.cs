using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class GameManagerScript : MonoBehaviourPunCallbacks
{
    [Header("Status")]
    public bool gameEnded = false;
    [Header("Players")]
    public string playerPrefabLocation;
    public string coinLocation;
    public Transform[] spawnPoints;

    public Transform[] spawnCoin;
    public PlayerController[] players;
    private int playersInGame;
    private List<int> pickedSpawnIndex;
    private List<int> itemPosition;
    [Header("Reference")]
    public GameObject imageTarget;
    //public DefaultObserverEventHandler defaultObserver;

    public int movHor, movVert;

    //instance
    public static GameManagerScript instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        pickedSpawnIndex = new List<int>();
        itemPosition = new List<int>();
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
        //DefaultObserverEventHandler.isTracking = false;
    }
    private void Update()
    {
        //Debug.Log("istracking" + DefaultObserverEventHandler.isTracking);
        foreach (GameObject gameObj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (gameObj.name == "Player(Clone)" || gameObj.name == "Coin(Clone)")
            {
                gameObj.transform.SetParent(imageTarget.transform);
            }
        }
        for (int i = 1; i < imageTarget.transform.childCount; i++)
        {
            //imageTarget.transform.GetChild(i).gameObject.SetActive(DefaultObserverEventHandler.isTracking);
        }

        if(itemPosition.Count != spawnCoin.Length){
            //photonView.RPC("SpawnItem", RpcTarget.All);
            SpawnItem();
        }
    }

    [PunRPC]
    void ImInGame()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            SpawnPlayer();
        }    
    }

    void SpawnPlayer()
    {
        int rand = Random.Range(0, spawnPoints.Length);
        while (pickedSpawnIndex.Contains(rand))
        {
            rand = Random.Range(0, spawnPoints.Length);
        }
        pickedSpawnIndex.Add(rand);
        GameObject playerObject = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[rand].position, Quaternion.identity);
        //intialize the player
        PlayerController playerScript = playerObject.GetComponent<PlayerController>();
        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    void SpawnItem(){
        Debug.Log("Spawn Item");
        int rand = Random.Range(0, spawnCoin.Length);
        while (itemPosition.Contains(rand))
        {
            rand = Random.Range(0, spawnCoin.Length);
        }
        itemPosition.Add(rand);
        GameObject item = PhotonNetwork.InstantiateRoomObject(coinLocation, spawnCoin[rand].position, Quaternion.identity);
    }

    public PlayerController GetPlayer(int playerID)
    {
        return players.First(x => x.id == playerID);
    }

    public PlayerController GetPlayer(GameObject playerObj)
    {
        return players.First(x => x.gameObject == playerObj);
    }
}
