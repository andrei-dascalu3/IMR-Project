using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSetupManager : MonoBehaviour
{
    public static PuzzleSetupManager instance;

    public Transform puzzle;
    public Transform backgroundPuzzle;

    public Texture puzzleTexture;
    public Texture backgroundPuzzleTexture;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                PuzzleSetupManager.instance = this;
                GameObject.Destroy(instance.gameObject);
            }
        }

    }
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