using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSetupManager : MonoBehaviour
{
    public Transform puzzle;
    public Transform backgroundPuzzle;

    public Texture puzzleTexture;
    public Texture backgroundPuzzleTexture;

    void Start()
    {
        ImportTexturePhotoToPuzzle(puzzle, puzzleTexture);
        ImportTexturePhotoToPuzzle(backgroundPuzzle, backgroundPuzzleTexture);
    }

    private void ImportTexturePhotoToPuzzle(Transform puzzle, Texture texture)
    {
        Renderer[] pieces = puzzle.GetComponentsInChildren<Renderer>();
        foreach (Renderer piece in pieces)
        {
            piece.material.EnableKeyword("_NORMALMAP");
            piece.material.SetTexture("_MainTex", texture);
        }
    }
}