using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSetupManager : MonoBehaviour
{
    public Texture puzzleTexture;
    void Start()
    {
        ImportTexturePhotoToPuzzle();
    }

    private void ImportTexturePhotoToPuzzle()
    {
        Renderer[] pieces = this.transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer piece in pieces)
        {
            Debug.Log(piece.name);
            piece.material.EnableKeyword("_NORMALMAP");
            piece.material.SetTexture("_MainTex", puzzleTexture);
        }
    }
}