using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager manager;
    public string gameversion = "0.02";
    public float sensetivety;

    string nicknameKey = "Nickname";
    string sensetivetyKey = "Sensetivety";

    [SerializeField] TMP_InputField nicknameField;
    [SerializeField] TMP_InputField roomNameField;
    [SerializeField] TMP_InputField sensetivetyField;
    [SerializeField] MenuManager menuManager;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text gameVersionField;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] RoomListItem roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] TMP_Text placeholderNicnameText;
    [SerializeField] TMP_Text placeholderSensText;

    private List<RoomListItem> _list = new List<RoomListItem>();

    private void Awake()
    {
        manager = this;
    }

    void Start()
    {
        //EnableLoadingMenu
        menuManager.Open(menuManager.menus[0]);

        LoadSettings();

        PhotonNetwork.GameVersion = gameversion;
        gameVersionField.text = PhotonNetwork.GameVersion;

        PhotonNetwork.ConnectUsingSettings();
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnConnectedToMaster()
    {
        menuManager.Open(menuManager.menus[1]);
        Debug.Log("Connected To Master");

        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
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


    public void SaveSettings()
    {
        PhotonNetwork.NickName = nicknameField.text;
        PlayerPrefs.SetString(nicknameKey, nicknameField.text);
        
        sensetivety = float.Parse(sensetivetyField.text);
        PlayerPrefs.SetFloat(sensetivetyKey, sensetivety);

        PlayerPrefs.Save();
    }
    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey(nicknameKey))
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString(nicknameKey);
        }
        else
        {
            PhotonNetwork.NickName = "Player " + Random.Range(1, 999);
        }
        placeholderNicnameText.text = PhotonNetwork.NickName;

        if (PlayerPrefs.HasKey(sensetivetyKey))
        {
            sensetivety = PlayerPrefs.GetFloat(sensetivetyKey);
        }
        else
        {
            sensetivety = 3;
        }
        placeholderSensText.text = sensetivety.ToString();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
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
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
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
