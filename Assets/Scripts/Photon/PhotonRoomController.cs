using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEngine.XR.Interaction.Toolkit;

public class PhotonRoomController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoomController room;
    public PhotonView pv;

    private string levelName;
    private string currentLevelName;

    private static string sceneName;
    //private static bool startedLevel = false;

    public string roomName;

    private void Awake()
    {
        if (PhotonRoomController.room == null)
        {
            PhotonRoomController.room = this;
        }
        else
        {
            if (PhotonRoomController.room != this)
            {
                GameObject.Destroy(PhotonRoomController.room.gameObject);
                PhotonRoomController.room = this;
            }
        }
        DontDestroyOnLoad(gameObject);
        pv = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        GameObject myRig = GameObject.Find(PhotonNetwork.NickName);
        myRig.SetActive(false);
        myRig.SetActive(true);

        CreatePlayer();

        currentLevelName = scene.name;
        if (currentLevelName != "MultiplayerLobby")
        {
            GameControllerMultiplayer gameController = GameObject.Find("GameController").GetComponent<GameControllerMultiplayer>();

            Transform handsParentTransform = myRig.transform.GetChild(1).GetChild(0);
            XRRayInteractor leftHandInteractor = handsParentTransform.GetChild(1).GetComponent<XRRayInteractor>();
            XRRayInteractor rightHandInteractor = handsParentTransform.GetChild(2).GetComponent<XRRayInteractor>();

            gameController.leftHand = leftHandInteractor;
            gameController.rightHand = rightHandInteractor;
        }

        sceneName = scene.name;
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating player avatar.");
        GameObject newAvatarGO = PhotonNetwork.Instantiate(Path.Combine("CharacterPrefab", "PhotonNetworkCharacter"), new Vector3(0, 2.15f, 0), Quaternion.identity, 0);
    }

    public override void OnJoinedRoom()
    {
        string name;

        foreach(Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            name = player.NickName;

            if (name == PhotonNetwork.NickName && !player.IsLocal)
            {
                //OTHER PLAYER WITH SAME NAME
                //Debug.Log("Exiting room, because nickname already exists.");
                //PhotonLobbyController.photonLobby.OnJoinFailUniqueNickname(name);
                //PhotonNetwork.LeaveRoom();
            }
        }

        base.OnJoinedRoom();
        Debug.Log("We have joined room");

        StartGameLobby();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Failed to join room");
        Debug.Log(message);

        PhotonLobbyController.photonLobby.AddErrorMessage(message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message + " error code: " + returnCode);

        if (returnCode == 32766)
        {
            PhotonLobbyController.photonLobby.AddErrorMessage("A room with this name already exists");
        }
        else
        {
            PhotonLobbyController.photonLobby.AddErrorMessage(message);
        }
       
    }

    public void StartGameLobby()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Going to multiplayer lobby");
            levelName = "MultiplayerLobby";
            PhotonNetwork.LoadLevel("MultiplayerLobby");
        }
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game Scene.");
            levelName = GetLevelName();
            PhotonNetwork.LoadLevel(levelName);
        }
    }

    private string GetLevelName()
    {
        return "Puzzle-" + PlayerSettingsData.instance.difficulty + "-Multiplayer";
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }
}
