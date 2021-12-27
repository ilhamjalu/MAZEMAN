using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviour
{
    public Button play;
    public GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            play.gameObject.SetActive(true);
        }
        else
        {
            play.gameObject.SetActive(false);
        }

        Debug.Log("Jumlah Player : " + PhotonNetwork.CurrentRoom.PlayerCount);


        if (PhotonNetwork.PlayerList.Length == 2)
        {
            player2.SetActive(true);
        }
    }

    public void PlayButton()
    {
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "InGame");
        //PhotonNetwork.LoadLevel("InGame");

    }
}
