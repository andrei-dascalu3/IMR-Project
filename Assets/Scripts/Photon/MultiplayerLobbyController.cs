using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class MultiplayerLobbyController : MonoBehaviourPunCallbacks
{
    public PhotonView pv;

    //public GameObject gameSettingsMenu;
    //public GameObject helpMenu;

    public SwitchDifficultyOptions difficultyOptions;
    public SwitchMusicOptions musicOptions;
    public SwitchTimerOptions timerOptions;
    public SwitchVolumeOptions volumeOptions;
    public SwipeMapMenu swipeMap;

    public List<Texture> allPuzzleTextures = new List<Texture>();

    private Material backGroundMaterial;
    private int textureIndex = -1;

    private void Start()
    {
        DontDestroyOnLoad(PlayerSettingsData.instance.gameObject);

        backGroundMaterial = GameObject.Find("Environment")
                                            .transform.GetChild(1)
                                            .GetComponent<Renderer>()
                                            .material;

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    gameSettingsMenu.SetActive(true);
        //}
        //else
        //{
        //    helpMenu.SetActive(true);
        //}

        pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void SyncBackgroundTexture(int photoIndex)
    {
        textureIndex = photoIndex;
        backGroundMaterial.mainTexture = allPuzzleTextures[photoIndex];
    }

    [PunRPC]
    public void SyncSettings(string difficulty, int musicValue, int volumeValue, int timerValue)
    {
        PlayerSettingsData.instance.puzzleTexture = allPuzzleTextures[textureIndex];
        PlayerSettingsData.instance.difficulty = difficulty;
        PlayerSettingsData.instance.musicValue = musicValue;
        PlayerSettingsData.instance.volumeValue = volumeValue;
        PlayerSettingsData.instance.timerValue = timerValue;

        PlayerSettingsData.instance.UpdateOwnGameObjects();
    }

    public void UpdatePlayerSettings()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerSettingsData.instance.puzzleTexture = swipeMap.GetSelectedPhoto();
            PlayerSettingsData.instance.difficulty = difficultyOptions.GetDifficulty();
            PlayerSettingsData.instance.musicValue = musicOptions.GetMusicValue();
            PlayerSettingsData.instance.volumeValue = volumeOptions.GetVolumeLevel();
            PlayerSettingsData.instance.timerValue = timerOptions.GetTimerValue();

            pv.RPC(nameof(SyncSettings), RpcTarget.Others,
                                                PlayerSettingsData.instance.difficulty,
                                                PlayerSettingsData.instance.musicValue,
                                                PlayerSettingsData.instance.volumeValue,
                                                PlayerSettingsData.instance.timerValue);
        }
    }

    public void OnStartButtonPress()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdatePlayerSettings();

            PhotonRoomController.room.StartGame();
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("SyncBackgroundTexture", RpcTarget.OthersBuffered, textureIndex);
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int currentTextureIndex = allPuzzleTextures.IndexOf(backGroundMaterial.mainTexture);
            if (textureIndex != currentTextureIndex)
            {
                textureIndex = currentTextureIndex;
                pv.RPC("SyncBackgroundTexture", RpcTarget.OthersBuffered, textureIndex);
            }

        }
    }
}
