using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField nicknameField;
    [SerializeField] TMP_InputField roomNameField;
    [SerializeField] MenuManager menuManager;
    void Start()
    {
        //EnableLoadingMenu
        menuManager.Open(menuManager.menus[0]);

        PhotonNetwork.NickName = "Player " + Random.Range(1, 999);

        PhotonNetwork.GameVersion = "0";
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
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReNicknameMe()
    {
        PhotonNetwork.NickName = nicknameField.text;
    }
}
