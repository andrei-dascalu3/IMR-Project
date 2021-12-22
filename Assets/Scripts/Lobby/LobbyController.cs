using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameModes
{
    SINGLE_PLAYER,
    MULTI_PLAYER_CREATE,
    MULTI_PLAYER_JOIN
}

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    public GameObject[] photonControllers;

    public SwitchDifficultyOptions difficultyOptions;
    public SwitchMusicOptions musicOptions;
    public SwitchTimerOptions timerOptions;
    public SwitchVolumeOptions volumeOptions;
    public SwipeMapMenu swipeMap;

    public GameModes gameMode;

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

        SceneManager.LoadScene("Puzzle-" + PlayerSettingsData.instance.difficulty, LoadSceneMode.Single);
    }

    public void OnSinglePlayerButtonPress()
    {
        GameObject photonMonoGO = GameObject.Find("PhotonMono");
        photonMonoGO.GetComponent<PhotonHandler>().ApplyDontDestroyOnLoad = false;
        gameMode = GameModes.SINGLE_PLAYER;
    }

    public void OnCreateRoomButtonPress()
    {
        for (int i = 0; i < photonControllers.Length; i++)
        {
            photonControllers[i].SetActive(true);
        }

        GameObject photonMonoGO = GameObject.Find("PhotonMono");
        photonMonoGO.GetComponent<PhotonHandler>().ApplyDontDestroyOnLoad = true;
        gameMode = GameModes.MULTI_PLAYER_CREATE;
    }

    public void OnJoinRoomButtonPress()
    {
        for (int i = 0; i < photonControllers.Length; i++)
        {
            photonControllers[i].SetActive(true);
        }
        GameObject photonMonoGO = GameObject.Find("PhotonMono");
        photonMonoGO.GetComponent<PhotonHandler>().ApplyDontDestroyOnLoad = true;
        gameMode = GameModes.MULTI_PLAYER_JOIN;
    }

    public void OnRoomSettingsCreateJoinButtonPress()
    {
        PhotonLobbyController photonLobby = photonControllers[0].GetComponent<PhotonLobbyController>();
        if (gameMode == GameModes.MULTI_PLAYER_JOIN)
        {
            photonLobby.ConnectToRoom();
        }
        else
        {
            if (gameMode == GameModes.MULTI_PLAYER_CREATE)
            {
                photonLobby.CreateRoom();
            }
        }
    }
}
