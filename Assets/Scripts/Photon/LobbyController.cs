using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject createJoinButtonsContainer;
    [SerializeField]
    private GameObject cancelConnectButton;

    public GameObject playerSettingsGO;

    public string roomName;//TO BE DELETED AFTER CLASS COMPLETION

    private void Awake()
    {
        DontDestroyOnLoad(playerSettingsGO);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        createJoinButtonsContainer.SetActive(true);
    }

    public void CreateRoom()
    {
        //int roomSize = GetRoomSize();     TODO
        //string roomName = GetRoomName();  TODO
        createJoinButtonsContainer.SetActive(false);
        cancelConnectButton.SetActive(true);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, /*MaxPlayers = (byte)roomSize;  TODO */ };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log("Starting to create room " + roomName);


        //VA FI CEVA DE ADAUGAT IN FUNCTIA OnCreateRoom??? 
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message);
        //EXISTA DEJA O CAMERA CU NUMELE ASTA??? TODO
    }

    public void ConnectToRoom()
    {
        //string roomName = GetRoomName(); TODO
        createJoinButtonsContainer.SetActive(false);
        cancelConnectButton.SetActive(true);
        PhotonNetwork.JoinRoom(roomName);
        Debug.Log("Starting to connect to room " + roomName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Failed to join room");
        Debug.Log(message);
        //TODO: verifica de ce nu s-a putut conecta in functie de returnCode
        //daca room e full, sau daca room nu exista
    }

    public void CancelConnectingToRoom()
    {
        cancelConnectButton.SetActive(false);
        createJoinButtonsContainer.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
