using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchMusicOptions : MonoBehaviour
{
    private int index;
    [SerializeField]
    private TextMeshProUGUI volumeOption;
    [SerializeField]
    private AudioSource musicManager;

    void Start()
    {
        if(musicManager == null)
        {
            musicManager = PlayerSettingsData.instance.GetComponent<AudioSource>();
        }

        index = 0;
        volumeOption.text = "On";
    }

    void UpdateMusicOptionText()
    {
        if (index < 0)
            index = 0;
        if (index > 1)
            index = 1;
        if (index == 0)
        {
            volumeOption.text = "On";
            musicManager.Play();
        }
        else
        {
            volumeOption.text = "Off";
            musicManager.Stop();
        }
    }
    public void Next()
    {
        index++;
        UpdateMusicOptionText();
    }
    public void Previous()
    {
        index--;
        UpdateMusicOptionText();
    }

    public int GetMusicValue()
    {
        return index;
    }
}
