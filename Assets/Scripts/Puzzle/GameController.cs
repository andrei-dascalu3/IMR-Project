using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public struct PieceOriginalWorldTranformData
{
    public Vector3 originalLocation;
    public Quaternion originalRotation;
}

public class GameController : MonoBehaviour
{
    private List<PieceOriginalWorldTranformData> piecesOriginalTransforms;

    private PlayerSettingsData playerSettings;

    public PuzzleSetupManager puzzleManager;

    public Transform puzzle;
    public Transform backgroundPuzzle;

    public Transform currentlyHoverdBackroundPiece;
    public Transform currentlyHeldPiece;

    private void Awake()
    {
        playerSettings = GameObject.Find("PlayerSettingsObject").GetComponent<PlayerSettingsData>();
        puzzleManager.puzzleTexture = playerSettings.puzzleTexture;


        SavePiecesOriginalLocations();
    }

    public void SavePiecesOriginalLocations()
    {
        piecesOriginalTransforms = new List<PieceOriginalWorldTranformData>();
        Transform[] pieces = puzzle.GetComponentsInChildren<Transform>();
        for (int i = 0; i < pieces.Length; i++)
        {
            PieceOriginalWorldTranformData ogTransform = new PieceOriginalWorldTranformData();
            ogTransform.originalLocation = pieces[i].position;
            ogTransform.originalRotation = pieces[i].rotation;
            piecesOriginalTransforms.Add(ogTransform);
        }
    }


    //every background puzzle piece should have 'XR Simple Interactable' component
    //this function should appear in the event 'Hover Enter' in the components of all those pieces
    public void OnBackgroundPuzzlePieceHoverEnter(HoverEnterEventArgs eventArgs)
    {
        currentlyHoverdBackroundPiece = eventArgs.interactable.transform;
    }

    public void OnBackgroundPuzzlePieceHoverExit(HoverExitEventArgs eventArgs)
    {
        if (eventArgs.interactable.transform == currentlyHoverdBackroundPiece)
        {
            currentlyHoverdBackroundPiece = null;
        }
    }

}
