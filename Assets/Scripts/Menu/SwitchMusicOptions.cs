using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchMusicOptions : MonoBehaviour
{
    int index;
    public TextMeshProUGUI volumeOption;
    void Start()
    {
        index = 0;
    }

    void Update()
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
    }
    public void Previous()
    {
        index--;

    }
}
