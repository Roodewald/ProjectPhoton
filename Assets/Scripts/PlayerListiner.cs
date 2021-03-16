using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListiner : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    Player playerInItem;

    public void SetUp(Player player)
    {
        playerInItem = player;
        text.text = player.NickName;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(otherPlayer == playerInItem)
        {
            Destroy(gameObject);
            Debug.Log("PlayerLeft");
        }
    }
}
