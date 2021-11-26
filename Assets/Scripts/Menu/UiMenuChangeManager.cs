using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMenuChangeManager : MonoBehaviour
{

    public GameObject gameModeButtons;

    public GameObject mainMenu;
    public GameObject settingsMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void UiToggleGameModeButtons(bool value)
    {
        gameModeButtons.SetActive(value);
    }

    public void UiToggleMainMenu(bool value)
    {
        mainMenu.SetActive(value);
    }

    public void UiToggleSettingsMenu(bool value)
    {
        settingsMenu.SetActive(value);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
