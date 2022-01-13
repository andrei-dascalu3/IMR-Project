using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonLobbyController : MonoBehaviourPunCallbacks
{
    public static PhotonLobbyController photonLobby;

    public TMP_InputField roomNameField;
    public TMP_InputField ownNameField;
    public TMP_InputField numberOfPlayersField;

    public TMP_Text multiplayerSettingsMenuErrorTextArea;

    public GameObject xrRigObject;

    private void Awake()
    {
        if (photonLobby == null)
        {
            photonLobby = this;
        }
        else
        {
            if (photonLobby != this)
            {
                Destroy(photonLobby.gameObject);
                photonLobby = this;
            }
        }

        DontDestroyOnLoad(xrRigObject);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to Photon master server.");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void CreateRoom()
    {
        int roomSize = GetRoomSize();
        string roomName = GetRoomName();

        PhotonNetwork.NickName = GetOwnName();
        xrRigObject.name = GetOwnName();

        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };

        PhotonRoomController.room.name = roomName;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log("Starting to create room " + roomName);
    }

    public void ConnectToRoom()
    {
        PhotonNetwork.NickName = GetOwnName();
        xrRigObject.name = GetOwnName();

        string roomName = GetRoomName();
        PhotonNetwork.JoinRoom(roomName);
        Debug.Log("Starting to connect to room " + roomName);
    }

    public void CancelConnectingToRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public int GetRoomSize()
    {
        return int.Parse(numberOfPlayersField.text);
    }
    public string GetRoomName()
    {
        return roomNameField.text;
    }
    private string GetOwnName()
    {
        return ownNameField.text;
    }

    public void OnJoinFailUniqueNickname(string name)
    {
        AddErrorMessage("There exists a player with the nickname " + name + " already.");
    }

    public void AddErrorMessage(string message)
    {
        multiplayerSettingsMenuErrorTextArea.gameObject.SetActive(true);
        multiplayerSettingsMenuErrorTextArea.text = message;
    }
}
