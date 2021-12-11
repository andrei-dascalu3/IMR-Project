using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerLobbyController : MonoBehaviourPunCallbacks
{
    public GameObject gameSettingsMenu;
    public GameObject helpMenu;

    public SwitchDifficultyOptions difficultyOptions;
    public SwitchMusicOptions musicOptions;
    public SwitchTimerOptions timerOptions;
    public SwitchVolumeOptions volumeOptions;
    public SwipeMapMenu swipeMap;

    private void Start()
    {
        DontDestroyOnLoad(PlayerSettingsData.instance.gameObject);

        if (PhotonNetwork.IsMasterClient)
        {
            gameSettingsMenu.SetActive(true);
        }
        else
        {
            helpMenu.SetActive(true);
        }
    }

    public void OnStartButtonPress()
    {
        PlayerSettingsData.instance.puzzleTexture = swipeMap.GetSelectedPhoto();
        PlayerSettingsData.instance.difficulty = difficultyOptions.GetDifficulty();
        PlayerSettingsData.instance.musicValue = musicOptions.GetMusicValue();
        PlayerSettingsData.instance.volumeValue = volumeOptions.GetVolumeLevel();
        PlayerSettingsData.instance.timerValue = timerOptions.GetTimerValue();

        PhotonRoomController.room.StartGame();
    }


    public void OnExitButtonPress()
    {

    }
}
