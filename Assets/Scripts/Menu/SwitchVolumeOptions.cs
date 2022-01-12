using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SwitchVolumeOptions : MonoBehaviour
{
    public GameObject musicManager;
    private int index;
    private int upperLimit;
    [SerializeField]
    private TextMeshProUGUI volumeText;
    void Start()
    {
        index = 10;
        upperLimit = 20;
        UpdateVolumeText();
    }

    private void Update()
    {
        musicManager.GetComponent<AudioSource>().volume = (float) index / 40F;
    }

    void UpdateVolumeText()
    {
        volumeText.text = index.ToString();
    }
    public void Next()
    {
        index++;
        if (index > upperLimit)
            index = upperLimit;
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
