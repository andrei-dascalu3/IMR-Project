using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerDataObject : MonoBehaviour
{
    public static LayerDataObject instance;

    public int puzzlePieceLayer;
    public int ungrabablePuzzlePieceLayer;

    public int ceilingLayer;

    void Awake()
    {
        instance = this;
    }
}
