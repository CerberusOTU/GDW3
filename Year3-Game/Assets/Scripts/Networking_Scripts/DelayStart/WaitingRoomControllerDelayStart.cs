using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingRoomControllerDelayStart : MonoBehaviourPunCallbacks
{
    //Photon view for sending rpc to update timer
    private PhotonView myPhotonView;

    [SerializeField]
    private int multiplayerSceneIndex;
    [SerializeField]
    private int menuSceneIndex;
    // number of players out of total
    private int playerCount;
    private int roomSize;
    [SerializeField]
    private int minPlayersToStart;

    // Text variables
    [SerializeField]
    private Text roomCountDisplay;
    [SerializeField]
    private Text timerToStartDisplay;

    // Bool for timer count down
    private bool readyToCountDown;
    private bool readyToStart;
    private bool startingGame;

    // CountDown timer variables
    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;

    // CountDown timer reset variables
    [SerializeField]
    private float maxWaitTime;
    [SerializeField]
    private float maxFullGameWaitTime;

    private void Start()
    {
        // Init variables
        myPhotonView = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        playerCountUpdate();

    }

    void playerCountUpdate()
    {
        // Updates player count
        // Displays player count
        // Triggers countdown timer
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        roomCountDisplay.text = playerCount + ":" + roomSize;

        if (playerCount == roomSize)
        {
            readyToStart = true;
        }
        else if (playerCount >= minPlayersToStart)
        {
            readyToCountDown = true;
        }
        else
        {
            readyToCountDown = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
            playerCountUpdate();

            if (PhotonNetwork.IsMasterClient)
            {
                myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
            }                   
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if (timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerCountUpdate();
    }

    private void Update()
    {
        WaitingForMorePlayers();
    }

    void WaitingForMorePlayers()
    {
        if (playerCount <= 1)
        {
            ResetTimer();
        }

        if (readyToStart)
        {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if (readyToCountDown)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;

        if (timerToStartGame <= 0f)
        {
            if (startingGame)
            {
                return;
            }
            startGame();
        }
    }

    void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxFullGameWaitTime;
    }

    void startGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }

    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}
