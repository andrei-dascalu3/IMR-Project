using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SwitchVolumeOptions : MonoBehaviour
{
    int index;
    public TextMeshProUGUI volumeText;
    void Start()
    {
        index = 0;
    }

    
    void Update()
    {
        volumeText.text = index.ToString();
    }
    public void Next()
    {
        index++;
        if (index > 100)
            index = 100;
    }
    public void Previous()
    {
        index--;
        if (index < 0)
            index = 0;
    }
}
