using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom(string roomName){
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2, IsVisible = true });
        Debug.Log("Create");
    }

    public void JoinRoom(string roomName){
        if(PhotonNetwork.PlayerList.Length <= 2){
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public void ChangeLocalScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    [PunRPC]
    public void ChangeScene(string scene){
        PhotonNetwork.LoadLevel(scene);
    }
}
