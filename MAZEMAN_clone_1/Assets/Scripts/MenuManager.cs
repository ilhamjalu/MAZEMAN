using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [Header("Menu")]
    public GameObject mainMenu;
    public GameObject createLobby;
    public GameObject lobbyMenu;

    [Header("Main Menu")]
    public Button btnCreateRoom;
    public Button btnJoinRoom;

    [Header("Lobby")]
    public Text roomName;
    public Text playerList;
    public Button btnPlay;

    // Start is called before the first frame update
    void Start()
    {
        btnCreateRoom.interactable = false;
        btnJoinRoom.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        btnCreateRoom.interactable = true;
        btnJoinRoom.interactable = true;

        PhotonNetwork.JoinLobby();
    }

    public void ShowPanelCreate()
    {
        createLobby.SetActive(true);
    }

    void SetMenu(GameObject menu){
        mainMenu.SetActive(false);
        lobbyMenu.SetActive(false);
        menu.SetActive(true);
    }

    public void CreateRoomBtn(Text InputRoomName){
        NetworkManager.instance.CreateRoom(InputRoomName.text);
        roomName.text = InputRoomName.text;
    }

    public void JoinRoomBtn(Text InputRoomName){
        NetworkManager.instance.JoinRoom(InputRoomName.text);
        roomName.text = InputRoomName.text;
    }

    public void PlayerNameUpdate(Text playerName){
        PhotonNetwork.NickName = playerName.text;
    }

    public override void OnJoinedRoom()
    {
        //SetMenu(lobbyMenu);
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);

        PhotonNetwork.JoinLobby();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdateLobbyUI();
    }

    [PunRPC]
    public void UpdateLobbyUI(){
        playerList.text = "";
        foreach(Player player in PhotonNetwork.PlayerList){
            if(player.IsMasterClient){
                playerList.text += player.NickName + " (Host) \n";
            }
            else{
                playerList.text += player.NickName + "\n";
            }

            if (PhotonNetwork.IsMasterClient){
                btnPlay.interactable = true;
            }
            else{
                btnPlay.interactable = false;
            }
        }
    }

    public void OnLeaveLobbyBtn()
    {
        PhotonNetwork.LeaveRoom();
        SetMenu(mainMenu);
    }

    [PunRPC]
    public void OnStartGameBtn()
    {
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }
}
