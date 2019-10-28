using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomControllerDelayStart : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int waitingRoomSceneIndex; //Number for the build index to the multiplayer scene

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
        SceneManager.LoadScene(waitingRoomSceneIndex);
    }
}
