using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom room;
    private PhotonView pv;

    public PlayerSettingsData settings;

    [SerializeField]
    private string levelName;

    [SerializeField]
    private string currentLevelName;

    private static string sceneName;
    private static bool startedLevel = false;

    private void Awake()
    {
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                GameObject.Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
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
        PhotonNetwork.Instantiate(Path.Combine("CharacterPrefab", "PhotonNetworkCharacter"), new Vector3(0, 2.15f, 0), Quaternion.identity, 0);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We have joined room");
        StartGame();

        if (startedLevel && sceneName == currentLevelName)
        {
            CreatePlayer();
        }
    }

    // Start is called before the first frame update
    void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            levelName = GetLevelName();
            PhotonNetwork.LoadLevel(levelName);
        }
    }

    private string GetLevelName()
    {
        return "Puzzle-" + settings.difficulty;
    }

    private void Update()
    {
        // Debug.Log(PhotonNetwork.PlayerList.Length);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }
}
