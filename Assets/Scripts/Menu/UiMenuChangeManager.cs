using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiMenuChangeManager : MonoBehaviour
{
    public GameObject gameModeButtons;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject multiplayerRoomSettingsMenu;

    public GameObject roomSettingsOnlyForCreate;
    public TMP_Text settingsRoomCreateJoinButtonText;

    public void OnPlayButtonPress()
    {
        gameModeButtons.SetActive(true);
    }

    //public void OnHelpButtonPress()
    //{

    //}

    //public void OnExitButtonPress()
    //{

    //}

    public void OnSinglePlayerButtonPress()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnCreateRoomButtonPress()
    {//if connected to master server???
        mainMenu.SetActive(false);
        multiplayerRoomSettingsMenu.SetActive(true);
        roomSettingsOnlyForCreate.SetActive(true);
        settingsRoomCreateJoinButtonText.text = "Create room";
    }

    public void OnJoinRoomButtonPress()
    {
        //if connected to master server???
        mainMenu.SetActive(false);
        multiplayerRoomSettingsMenu.SetActive(true);
        roomSettingsOnlyForCreate.SetActive(false);
        settingsRoomCreateJoinButtonText.text = "Join room";
    }

    public void OnRoomSettingsCreateButtonPress()
    {
        string gameMode = LobbyController.instance.gameMode;
        if (gameMode == "MPC")
        {
            settingsMenu.SetActive(true);
            multiplayerRoomSettingsMenu.SetActive(false);
        }

    }
}
