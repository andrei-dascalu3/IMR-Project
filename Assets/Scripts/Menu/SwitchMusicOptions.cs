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
            volumeOption.text = "On";
        else volumeOption.text = "Off";
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
