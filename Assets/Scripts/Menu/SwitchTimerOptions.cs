using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchTimerOptions : MonoBehaviour
{
    private int index;
    [SerializeField]
    private TextMeshProUGUI timerText;
    void Start()
    {
        index = 0;
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
        index += 300;
        if (index > 1800)
            index = 1800;
        UpdateTimerText();
    }
    public void Prevoius()
    {
        index -= 300;
        if (index < 0)
            index = 0;
        UpdateTimerText();
    }

    public int GetTimerValue()
    {
        return index;
    }
}
