using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SwitchVolumeOptions : MonoBehaviour
{
    private int index;
    [SerializeField]
    private TextMeshProUGUI volumeText;
    void Start()
    {
        index = 0;
        UpdateVolumeText();
    }

    void UpdateVolumeText()
    {
        volumeText.text = index.ToString();
    }
    public void Next()
    {
        index++;
        if (index > 100)
            index = 100;
        UpdateVolumeText();
    }
    public void Previous()
    {
        index--;
        if (index < 0)
            index = 0;
        UpdateVolumeText();
    }

    public int GetVolumeLevel()
    {
        return index;
    }
}
