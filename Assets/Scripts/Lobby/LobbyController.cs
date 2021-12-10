using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    public GameObject[] photonControllers;

    public SwitchDifficultyOptions difficultyOptions;
    public SwitchMusicOptions musicOptions;
    public SwitchTimerOptions timerOptions;
    public SwitchVolumeOptions volumeOptions;
    public SwipeMapMenu swipeMap;

    public string gameMode;

    private void Awake()
    {
        if (LobbyController.instance == null)
        {
            LobbyController.instance = this;
        }
        else
        {
            if (LobbyController.instance != this)
            {
                GameObject.Destroy(LobbyController.instance.gameObject);
                LobbyController.instance = this;
            }
        }

    }

    public void OnStartGameButtonPress()
    {
        DontDestroyOnLoad(PlayerSettingsData.instance.gameObject);
        PlayerSettingsData.instance.puzzleTexture = swipeMap.GetSelectedPhoto();
        PlayerSettingsData.instance.difficulty = difficultyOptions.GetDifficulty();
        PlayerSettingsData.instance.musicValue = musicOptions.GetMusicValue();
        PlayerSettingsData.instance.volumeValue = volumeOptions.GetVolumeLevel();
        PlayerSettingsData.instance.timerValue = timerOptions.GetTimerValue();


        if (gameMode == "SP")
        {
            SceneManager.LoadScene("Puzzle-" + PlayerSettingsData.instance.difficulty, LoadSceneMode.Single);
        }
        else
        {
            if (gameMode == "MPC")
            {
                PhotonLobby photonLobby = photonControllers[0].GetComponent<PhotonLobby>();
                photonLobby.CreateRoom();
            }
        }
    }

    public void OnSinglePlayerButtonPress()
    {
        GameObject photonMonoGO = GameObject.Find("PhotonMono");
        photonMonoGO.GetComponent<PhotonHandler>().ApplyDontDestroyOnLoad = false;
        gameMode = "SP";
    }

    public void OnCreateRoomButtonPress()
    {
        for (int i = 0; i < photonControllers.Length; i++)
        {
            photonControllers[i].SetActive(true);
        }

        GameObject photonMonoGO = GameObject.Find("PhotonMono");
        photonMonoGO.GetComponent<PhotonHandler>().ApplyDontDestroyOnLoad = true;
        gameMode = "MPC";
    }

    public void OnJoinRoomButtonPress()
    {
        for (int i = 0; i < photonControllers.Length; i++)
        {
            photonControllers[i].SetActive(true);
        }
        GameObject photonMonoGO = GameObject.Find("PhotonMono");
        photonMonoGO.GetComponent<PhotonHandler>().ApplyDontDestroyOnLoad = true;
        gameMode = "MPJ";
    }

    public void OnRoomSettingsCreateButtonPress()
    {
        if (gameMode == "MPJ")
        {
            PhotonLobby photonLobby = photonControllers[0].GetComponent<PhotonLobby>();
            photonLobby.ConnectToRoom();
        }
    }

    //public void OnHelpButtonPress()
    //{

    //}

    //public void OnExitButtonPress()
    //{

    //}
}
