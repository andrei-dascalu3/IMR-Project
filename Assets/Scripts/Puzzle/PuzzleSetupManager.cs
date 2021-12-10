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

    //public PlayerSettingsData playerSetting;

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
        //playerSetting = GameObject.Find("PlayerSettingsObject").GetComponent<PlayerSettingsData>();

    }
    void Start()
    {
        //puzzleTexture = playerSetting.puzzleTexture;

        puzzleTexture = PlayerSettingsData.instance.puzzleTexture;
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