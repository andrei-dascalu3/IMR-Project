using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchDifficultyOptions : MonoBehaviour
{
    private int index;
    private string[] difficultyOptions = { "Easy", "Medium", "Hard" };
    [SerializeField]
    private TextMeshProUGUI difficultyText;
    void Start()
    {
        index = 0;
        UpdateDifficultyText();
    }

    void UpdateDifficultyText()
    {
        difficultyText.text = difficultyOptions[index];
    }
    public void Next()
    {
        index++;
        if (index > difficultyOptions.Length - 1)
            index = 0;
        UpdateDifficultyText();
    }
    public void Prevoius()
    {
        index--;
        if (index < 0)
            index = difficultyOptions.Length - 1;
        UpdateDifficultyText();
    }

    public string GetDifficulty()
    {
        return difficultyOptions[index];
    }
}
