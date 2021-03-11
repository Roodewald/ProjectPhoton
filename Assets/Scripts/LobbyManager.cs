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
    [SerializeField] GameObject roomListItemPrefab;

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
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameField.text))
        {
            return;
        }
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
        // foreach (Transform trans in roomListContent)
        //  {
        //    Destroy(trans.gameObject);
        //  }
        Debug.Log("FindRoom!!!!");
        for (int i = 0; i < roomList.Count; i++)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public void JoinLobby(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        menuManager.Open(menuManager.menus[0]);
    }

    public override void OnJoinedRoom()
    {
        menuManager.Open(menuManager.menus[5]);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    public void LeaveRoom()
    {
        menuManager.Open(menuManager.menus[1]);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        menuManager.Open(menuManager.menus[1]);
    }
}