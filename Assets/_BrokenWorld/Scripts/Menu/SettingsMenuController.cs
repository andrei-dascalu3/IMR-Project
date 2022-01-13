using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenuController : MonoBehaviour
{
    public void LoadGame(Texture image)
    {
        BackgroundImageController.backgroundImage = image;
        SceneManager.LoadScene("BrokenWorldLobby");
    }
}
