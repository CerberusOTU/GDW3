using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControllerQuickStart : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int multiplayerSceneIndex; //Number for the build index to the multiplayer scene

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom() //Callback for when successfully join or create room
    {
        Debug.Log("Joined Room");
        StartGame();
    }

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(multiplayerSceneIndex); // Because of AutoSyncScene
        }
    }
}
