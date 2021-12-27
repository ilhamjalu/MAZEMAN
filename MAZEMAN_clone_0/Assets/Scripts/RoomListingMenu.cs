using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    public Transform content;

    public RoomListing roomListing;

    private List<RoomListing> listingList = new List<RoomListing>();

    void Update()
    {
        Debug.Log(listingList.Count);
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("TEST");
        foreach(RoomInfo info in roomList)
        {
            if(info.RemovedFromList)
            {
                Debug.Log("test 1");
                int index = listingList.FindIndex(x => x.room.Name == info.Name);
                if(index != -1)
                {
                    Destroy(listingList[index].gameObject);
                    listingList.RemoveAt(index);
                }
            }
            else
            {
                Debug.Log("test 2");

                RoomListing listing = Instantiate(roomListing, content);
                if(listing != null)
                {
                    listing.SetRoomInfo(info);
                    listingList.Add(listing);
                }
            }
        }        
    }
}
