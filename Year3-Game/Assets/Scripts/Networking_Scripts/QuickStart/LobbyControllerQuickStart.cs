using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyControllerQuickStart : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject quickStartButton; // Button used for joining a game
    [SerializeField]
    private GameObject quickCancelButton; // Button used to stop searching for game to join
    [SerializeField]
    private int roomSize; //Manual set the number of player inn room at one time

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        quickStartButton.SetActive(true);
    }

    public void QuickStart()
    {
        quickStartButton.SetActive(false);
        quickCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom(); // Tries to join existing room
        Debug.Log("Quick Start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) //Callback function for failed room join
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0,10000); // Creating name for room
        RoomOptions roomOps = new RoomOptions() {IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize};
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps); // Attempting to create new room
        Debug.Log(randomRoomNumber);    
    }

    public override void OnCreateRoomFailed(short returnCode, string message) // Callback for failed create room
    {
        Debug.Log("Failed to create room...trying again");
        CreateRoom();
    }

    public void QuickCancel() // Stop looking for a room, paired to cancel button
    {
        quickCancelButton.SetActive(false);
        quickStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
