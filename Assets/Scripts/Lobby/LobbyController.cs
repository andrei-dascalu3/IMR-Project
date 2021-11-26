using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public GameObject[] photonControllers;

    public PlayerSettingsData playerSettings;

    public SwitchDifficultyOptions difficultyOptions;
    public SwitchMusicOptions musicOptions;
    public SwitchTimerOptions timerOptions;
    public SwitchVolumeOptions volumeOptions;
    public SwipeMapMenu swipeMap;

    public string gameMode;

    private void Awake()
    {
        DontDestroyOnLoad(playerSettings);
    }

    public void OnStartGameButtonPress()
    {
        playerSettings.puzzleTexture = swipeMap.GetSelectedPhoto();
        playerSettings.difficulty = difficultyOptions.GetDifficulty();
        playerSettings.musicValue = musicOptions.GetMusicValue();
        playerSettings.volumeValue = volumeOptions.GetVolumeLevel();
        playerSettings.timerValue = timerOptions.GetTimerValue();


        if (gameMode == "SP")
        {
            SceneManager.LoadScene("Puzzle-" + playerSettings.difficulty, LoadSceneMode.Single);
        }
        else
        {
            PhotonLobby photonLobby = photonControllers[0].GetComponent<PhotonLobby>();
            if (gameMode == "MPC")
            {
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
