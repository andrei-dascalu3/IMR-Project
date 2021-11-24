using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchDifficultyOptions : MonoBehaviour
{
    int index;
    string[] difficultyOptions = { "Easy", "Medium", "Hard"};
    public TextMeshProUGUI difficultyText;
    void Start()
    {
        index = 0;
    }

    void Update()
    {
        difficultyText.text = difficultyOptions[index];
    }
    public void Next()
    {
        index++;
        if (index > 2)
            index = 2;
    }
    public void Prevoius()
    {
        index--;
        if (index < 0)
            index = 0;
    }
}
