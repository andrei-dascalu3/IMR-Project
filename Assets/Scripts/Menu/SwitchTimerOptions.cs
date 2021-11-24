using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchTimerOptions : MonoBehaviour
{
    int index;
    public TextMeshProUGUI timerText;
    void Start()
    {
        index = 0;
    }

    void Update()
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
    }
    public void Prevoius()
    {
        index-=300;
        if (index < 0)
            index = 0;
    }
}
