using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
   
    RoomInfo info;

    public void SetUp(RoomInfo roomInfo)
    {
        info = roomInfo;
        text.text = roomInfo.Name;
    }
    public void OnClick()
    {
        LobbyManager.manager.JoinRoom(info);
    }
}
