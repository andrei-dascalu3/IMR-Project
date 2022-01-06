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
    private GameObject musicManager;

    void Start()
    {
        index = 0;
        UpdateMusicOptionText();
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
            musicManager.GetComponent<AudioSource>().Play();
        }
        else
        {
            volumeOption.text = "Off";
            musicManager.GetComponent<AudioSource>().Stop();
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
