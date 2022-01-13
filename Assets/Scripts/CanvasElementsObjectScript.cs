using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasElementsObjectScript : MonoBehaviour
{
    public static CanvasElementsObjectScript playerCanvas;

    public GameObject timerCanvas;
    public TextMeshProUGUI timerText;

    public TextMeshPro winText;
    public TextMeshPro loseText;

    public void Awake()
    {
        playerCanvas = this;
    }
}
