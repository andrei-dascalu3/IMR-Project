using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby photonLobby;

    public TMP_InputField roomNameField;
    public TMP_InputField ownNameField;
    public TMP_InputField numberOfPlayersField;

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
        PhotonNetwork.NickName = GetOwnName();
        xrRigObject.name = GetOwnName();

        string roomName = GetRoomName();
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
}
