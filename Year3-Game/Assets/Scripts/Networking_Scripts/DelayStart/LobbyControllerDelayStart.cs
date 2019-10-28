using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyControllerDelayStart : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject delayStartButton; // Button used for joining a game
    [SerializeField]
    private GameObject delayCancelButton; // Button used to stop searching for game to join
    [SerializeField]
    private int roomSize; //Manual set the number of player inn room at one time

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        delayStartButton.SetActive(true);
    }

    public void DelayStart()
    {
        delayStartButton.SetActive(false);
        delayCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom(); // Tries to join existing room
        Debug.Log("Delay Start");
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

    public void DelayCancel() // Stop looking for a room, paired to cancel button
    {
        delayCancelButton.SetActive(false);
        delayStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
