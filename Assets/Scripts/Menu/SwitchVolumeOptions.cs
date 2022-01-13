using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SwitchVolumeOptions : MonoBehaviour
{
    public AudioSource musicManager;
    private int index;
    private int upperLimit;
    [SerializeField]
    private TextMeshProUGUI volumeText;
    void Start()
    {
        if(musicManager == null)
        {
            musicManager = PlayerSettingsData.instance.GetComponent<AudioSource>();
            index = 10;
            musicManager.volume = (float)index / 40F;
        }
        else
        {
            index = 10;
        }
        
        upperLimit = 20;
        UpdateVolumeText();
    }

    //private void Update()
    //{
    //    musicManager.volume = (float) index / 40F;
    //}

    void UpdateVolumeText()
    {
        volumeText.text = index.ToString();
    }
    public void Next()
    {
        index++;
        if (index > upperLimit)
            index = upperLimit;

        musicManager.volume = (float)index / 40F;
        UpdateVolumeText();
    }
    public void Previous()
    {
        index--;
        if (index < 0)
            index = 0;

        musicManager.volume = (float)index / 40F;
        UpdateVolumeText();
    }

    public int GetVolumeLevel()
    {
        return index;
    }
}
