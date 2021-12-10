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

        GameObject myRig = GameObject.Find(PhotonNetwork.NickName);
        myRig.SetActive(false);
        myRig.SetActive(true);

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        if (PhotonNetwork.IsMasterClient)
        {
            gameSettingsMenu.SetActive(true);
        }
        else
        {
            helpMenu.SetActive(true);
        }

        DontDestroyOnLoad(PlayerSettingsData.instance.gameObject);
    }

    public void OnStartButtonPress()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerSettingsData.instance.puzzleTexture = swipeMap.GetSelectedPhoto();
            PlayerSettingsData.instance.difficulty = difficultyOptions.GetDifficulty();
            PlayerSettingsData.instance.musicValue = musicOptions.GetMusicValue();
            PlayerSettingsData.instance.volumeValue = volumeOptions.GetVolumeLevel();
            PlayerSettingsData.instance.timerValue = timerOptions.GetTimerValue();

            Debug.Log("Starting Game Scene.");
            string levelName = GetLevelName();
            PhotonNetwork.LoadLevel(levelName);
        }
    }

    private string GetLevelName()
    {
        return "Puzzle-" + PlayerSettingsData.instance.difficulty + "-Multiplayer";
    }

    public void OnExitButtonPress()
    {

    }
}
