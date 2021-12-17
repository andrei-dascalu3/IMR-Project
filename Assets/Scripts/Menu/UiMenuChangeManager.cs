using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiMenuChangeManager : MonoBehaviour
{
    public GameObject exitButton;
    public GameObject gameModeButtons;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject helpMenu;
    public GameObject multiplayerRoomSettingsMenu;

    public GameObject roomSettingsOnlyForCreate;
    public TMP_Text settingsRoomCreateJoinButtonText;

    public void OnPlayButtonPress()
    {
        gameModeButtons.SetActive(true);
    }

    public void OnHelpButtonPress()
    {
        mainMenu.SetActive(false);
        helpMenu.SetActive(true);
    }

    public void OnSinglePlayerButtonPress()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnCreateRoomButtonPress()
    {
        mainMenu.SetActive(false);
        multiplayerRoomSettingsMenu.SetActive(true);
        roomSettingsOnlyForCreate.SetActive(true);
        settingsRoomCreateJoinButtonText.text = "Create room";
    }

    public void OnJoinRoomButtonPress()
    {
        mainMenu.SetActive(false);
        multiplayerRoomSettingsMenu.SetActive(true);
        roomSettingsOnlyForCreate.SetActive(false);
        settingsRoomCreateJoinButtonText.text = "Join room";
    }

    public void OnRoomSettingsCreateButtonPress()
    {
        GameModes gameMode = LobbyController.instance.gameMode;
        if (gameMode == GameModes.MULTI_PLAYER_CREATE)
        {
            settingsMenu.SetActive(true);
            multiplayerRoomSettingsMenu.SetActive(false);
        }
    }
}
