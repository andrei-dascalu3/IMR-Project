using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

[System.Serializable]
public struct PieceTranformData
{
    public Vector3 position;
    public Quaternion rotation;
}

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public List<PieceTranformData> piecesOriginalTransforms;

    public PuzzlePiecesMovementManager puzzleMovementManager;

    [SerializeField]
    protected Transform currentlyHoverdBackroundPiece;
    [SerializeField]
    protected Transform currentlyHeldPiece;

    public List<Transform> puzzlePiecesTransforms;
    public List<Transform> backgroundPiecesTransforms;

    public XRRayInteractor leftHand;
    public XRRayInteractor rightHand;

    protected int piecesPlacedCorrectly = 0;

    public GameObject winMessage;

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        puzzlePiecesTransforms = new List<Transform>(PuzzleSetupManager.instance.puzzle.GetComponentsInChildren<Transform>());
        puzzlePiecesTransforms.RemoveAt(0);

        backgroundPiecesTransforms = new List<Transform>(PuzzleSetupManager.instance.backgroundPuzzle.GetComponentsInChildren<Transform>());
        backgroundPiecesTransforms.RemoveAt(0);

        SavePiecesOriginalLocations();
    }

    public virtual void Start()
    {
        SubscribeMethodsToXrEvents();
    }

    public void SavePiecesOriginalLocations()
    {
        piecesOriginalTransforms = new List<PieceTranformData>();
        for (int i = 0; i < puzzlePiecesTransforms.Count; i++)
        {
            PieceTranformData ogTransform = new PieceTranformData();
            ogTransform.position = puzzlePiecesTransforms[i].transform.position;
            ogTransform.rotation = puzzlePiecesTransforms[i].transform.rotation;
            piecesOriginalTransforms.Add(ogTransform);
        }
    }

    private void SubscribeMethodsToXrEvents()
    {
        leftHand.selectEntered.AddListener(OnGrabEnter);
        leftHand.selectExited.AddListener(OnGrabExit);
        rightHand.selectEntered.AddListener(OnGrabEnter);
        rightHand.selectExited.AddListener(OnGrabExit);

        leftHand.hoverEntered.AddListener(OnHoverEnter);
        leftHand.hoverExited.AddListener(OnHoverExit);
        rightHand.hoverEntered.AddListener(OnHoverEnter);
        rightHand.hoverExited.AddListener(OnHoverExit);
    }

    public void OnHoverEnterBackgroundPuzzlePiece(HoverEnterEventArgs eventArgs)
    {
        currentlyHoverdBackroundPiece = eventArgs.interactable.transform;
    }

    public void OnHoverExitBackgroundPuzzlePiece(HoverExitEventArgs eventArgs)
    {
        //if (eventArgs.interactable.transform == currentlyHoverdBackroundPiece)
        //{
        //    currentlyHoverdBackroundPiece = null;
        //}
    }

    public virtual void OnGrabEnter(SelectEnterEventArgs args)
    {
        currentlyHeldPiece = args.interactable.transform;
        currentlyHeldPiece.gameObject.layer = 2;
    }

    public virtual void OnGrabExit(SelectExitEventArgs args)
    {

        currentlyHeldPiece.gameObject.layer = 0;
        if (currentlyHoverdBackroundPiece == null)
        {
            return;
        }
        if (currentlyHeldPiece == null)
        {
            return;
        }

        TryPlacePiece(currentlyHeldPiece, currentlyHoverdBackroundPiece);

        currentlyHeldPiece = null;
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        XRSimpleInteractable simpleInteractable = args.interactable.GetComponent<XRSimpleInteractable>();
        if (simpleInteractable != null)
        {
            OnHoverEnterBackgroundPuzzlePiece(args);
            return;
        }
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        XRSimpleInteractable simpleInteractable = args.interactable.GetComponent<XRSimpleInteractable>();
        if (simpleInteractable != null)
        {
            OnHoverExitBackgroundPuzzlePiece(args);
            return;
        }
    }

    public virtual void TryPlacePiece(Transform pieceToPlace, Transform backgroundPieceWherePlaced)
    {
        bool isCorrect;

        int indexPieceToPlace = puzzlePiecesTransforms.IndexOf(pieceToPlace);
        int indexPieceWherePlaced = backgroundPiecesTransforms.IndexOf(backgroundPieceWherePlaced);

        isCorrect = (indexPieceToPlace == indexPieceWherePlaced);

        PieceTranformData placeToPutPiece = piecesOriginalTransforms[indexPieceWherePlaced];

        puzzleMovementManager.PlacePiece(pieceToPlace, placeToPutPiece, isCorrect);

        if (isCorrect)
        {
            OnPieceCorrectPlace(indexPieceToPlace);

            if (piecesPlacedCorrectly == puzzlePiecesTransforms.Count)
            {
                WinAction();
            }
        }
    }

    public virtual void OnPieceCorrectPlace(int indexPieceToPlace)
    {
        /*piecesPlacedCorrectly++;*/
        // for testing purposes
        piecesPlacedCorrectly = puzzlePiecesTransforms.Count;
    }

    public virtual void WinAction()
    {
        winMessage.SetActive(true);
    }
}
