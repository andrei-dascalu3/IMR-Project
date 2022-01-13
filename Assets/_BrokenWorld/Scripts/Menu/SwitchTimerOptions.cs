using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchTimerOptions : MonoBehaviour
{
    private int index;
    private const int volumeStep = 300;
    private const int maxVolume = 1800;
    [SerializeField]
    private TextMeshProUGUI timerText;

    void Start()
    {
        index = 0;
        //volumeStep = 300;
        //maxVolume = 1800;
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        if (index == 0)
            timerText.text = "None";
        else timerText.text = index.ToString() + " seconds";
    }
    public void Next()
    {
        index += volumeStep;
        if (index > maxVolume)
            index = maxVolume;
        UpdateTimerText();
    }
    public void Previous()
    {
        index -= volumeStep;
        if (index < 0)
            index = 0;
        UpdateTimerText();
    }

    public int GetTimerValue()
    {
        return index;
    }
}
