using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class PhotonRoomController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoomController room;
    private PhotonView pv;

    public PlayerSettingsData settings;

    [SerializeField]
    private string levelName;

    [SerializeField]
    private string currentLevelName;

    private static string sceneName;
    private static bool startedLevel = false;

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

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {

        currentLevelName = scene.name;
        if (currentLevelName == levelName)
        {
            CreatePlayer();
        }

        startedLevel = true;
        sceneName = scene.name;
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating player avatar.");
        GameObject newAvatarGO = PhotonNetwork.Instantiate(Path.Combine("CharacterPrefab", "PhotonNetworkCharacter"), new Vector3(0, 2.15f, 0), Quaternion.identity, 0);

        DontDestroyOnLoad(newAvatarGO);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We have joined room");
        StartGameLobby();

        if (startedLevel && sceneName == currentLevelName)
        {
            CreatePlayer();
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

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }
}
