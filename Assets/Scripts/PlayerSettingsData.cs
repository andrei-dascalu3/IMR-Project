using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsData : MonoBehaviour
{
    public Texture puzzleTexture;

    public string difficulty;
    public int volumeValue;
    public int musicValue;
    public int timerValue;

    // Start is called before the first frame update
    void Start()
    {
        //puzzleTexture = BackgroundImageController.backgroundImage;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
