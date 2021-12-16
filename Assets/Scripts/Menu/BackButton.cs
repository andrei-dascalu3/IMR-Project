using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public GameObject currentMenu;
    public GameObject mainMenu;

    public void OnBackButtonPress()
    {
        currentMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
