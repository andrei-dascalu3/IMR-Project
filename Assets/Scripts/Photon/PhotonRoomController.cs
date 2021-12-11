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
    private PhotonView pv;

    private string levelName;
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

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        GameObject myRig = GameObject.Find(PhotonNetwork.NickName);
        myRig.SetActive(false);
        myRig.SetActive(true);

        currentLevelName = scene.name;
        if (currentLevelName == "MultiplayerLobby")
        {
            //CreatePlayer();
        }
        else
        {
            GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();

            Transform handsParentTransform = myRig.transform.GetChild(1).GetChild(0);
            XRRayInteractor leftHandInteractor = handsParentTransform.GetChild(1).GetComponent<XRRayInteractor>();
            XRRayInteractor rightHandInteractor = handsParentTransform.GetChild(2).GetComponent<XRRayInteractor>();

            gameController.leftHand = leftHandInteractor;
            gameController.rightHand = rightHandInteractor;
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

        //if (startedLevel && sceneName == currentLevelName)
        //{
        CreatePlayer();
        //}
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
