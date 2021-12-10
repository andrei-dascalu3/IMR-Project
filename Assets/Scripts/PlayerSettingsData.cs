using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsData : MonoBehaviour
{
    public static PlayerSettingsData instance;

    public Texture puzzleTexture;

    public string difficulty;
    public int volumeValue;
    public int musicValue;
    public int timerValue;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (this != instance)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
