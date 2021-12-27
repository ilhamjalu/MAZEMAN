using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class RoomListing : MonoBehaviour
{
    public TextMeshProUGUI text;

    public RoomInfo room{ get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        room = roomInfo;
        text.text = roomInfo.Name;
    }

    public void JoinRoomBtn(TextMeshProUGUI InputRoomName)
    {
        NetworkManager.instance.JoinRoom(InputRoomName.text);
        NetworkManager.instance.ChangeLocalScene("Lobby");
    }
}
