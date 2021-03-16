﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager manager;

    [SerializeField] TMP_InputField nicknameField;
    [SerializeField] TMP_InputField roomNameField;
    [SerializeField] MenuManager menuManager;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text errorText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] RoomListItem roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;

    private List<RoomListItem> _list = new List<RoomListItem>();

    private void Awake()
    {
        manager = this;
    }

    void Start()
    {
        //EnableLoadingMenu
        menuManager.Open(menuManager.menus[0]);

        PhotonNetwork.NickName = "Player " + Random.Range(1, 999);

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        menuManager.Open(menuManager.menus[1]);
        Debug.Log("Connected To Master");

        PhotonNetwork.JoinLobby();
    }
    public void QuitGame()
    {
        Debug.Log("Application Quit");
        Application.Quit();

        PhotonNetwork.LeaveLobby();
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameField.text))
        {
            return;
        }
        menuManager.Open(menuManager.menus[0]);
        PhotonNetwork.CreateRoom(roomNameField.text);
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        menuManager.Open(menuManager.menus[4]);
        errorText.text = "Create room failed " + message + " With code :" + returnCode;
    }

    public void ReNicknameMe()
    {
        PhotonNetwork.NickName = nicknameField.text;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log(roomList);
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = _list.FindIndex(x => x.info.Name == info.Name);
                if (index != -1)
                {
                    Destroy(_list[index].gameObject);
                    _list.RemoveAt(index);
                }
            }
            else
            {
                Debug.Log(info);
                RoomListItem listing = Instantiate(roomListItemPrefab, roomListContent);
                if (listing != null)
                    listing.SetUp(info);
                _list.Add(listing);
            }
        }
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        menuManager.Open(menuManager.menus[0]);
    }

    public override void OnJoinedRoom()
    {
        menuManager.Open(menuManager.menus[5]);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListiner>().SetUp(players[i]);
        }
    }

    public void LeaveRoom()
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        menuManager.Open(menuManager.menus[0]);
        PhotonNetwork.LeaveRoom();
        
    }
    public override void OnLeftRoom()
    {
        menuManager.Open(menuManager.menus[1]);
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListiner>().SetUp(newPlayer);
    }
 
}
