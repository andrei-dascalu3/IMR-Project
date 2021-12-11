using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public struct PieceOriginalWorldTranformData
{
    public Vector3 originalLocation;
    public Quaternion originalRotation;
}

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private List<PieceOriginalWorldTranformData> piecesOriginalTransforms;

    [SerializeField]
    private Transform currentlyHoverdBackroundPiece;
    [SerializeField]
    private Transform currentlyHeldPiece;

    public List<Transform> puzzlePiecesTransforms;
    public List<Transform> backgroundPiecesTransforms;

    public XRRayInteractor leftHand;
    public XRRayInteractor rightHand;

    private int piecesPlacedCorrectly = 0;

    public GameObject winMessage;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //else
        //{
        //    if (instance != this)
        //    {
        //        GameController.instance = this;
        //        GameObject.Destroy(instance.gameObject);
        //    }
        //}

        puzzlePiecesTransforms = new List<Transform>(PuzzleSetupManager.instance.puzzle.GetComponentsInChildren<Transform>());
        backgroundPiecesTransforms = new List<Transform>(PuzzleSetupManager.instance.backgroundPuzzle.GetComponentsInChildren<Transform>());

        SavePiecesOriginalLocations();
    }

    private void Start()
    {
        SubscribeMethodsToXrEvents();
    }

    public void SavePiecesOriginalLocations()
    {
        piecesOriginalTransforms = new List<PieceOriginalWorldTranformData>();
        for (int i = 0; i < puzzlePiecesTransforms.Count; i++)
        {
            PieceOriginalWorldTranformData ogTransform = new PieceOriginalWorldTranformData();
            ogTransform.originalLocation = puzzlePiecesTransforms[i].transform.position;
            ogTransform.originalRotation = puzzlePiecesTransforms[i].transform.rotation;
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
        if (eventArgs.interactable.transform == currentlyHoverdBackroundPiece)
        {
            currentlyHoverdBackroundPiece = null;
        }
    }

    public void OnGrabEnter(SelectEnterEventArgs args)
    {
        currentlyHeldPiece = args.interactable.transform;
        currentlyHeldPiece.gameObject.layer = 2;
    }

    public void OnGrabExit(SelectExitEventArgs args)
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

    private void TryPlacePiece(Transform pieceToPlace, Transform backgroundPieceWherePlaced)
    {
        bool isCorrect;

        int indexPieceToPlace = puzzlePiecesTransforms.IndexOf(pieceToPlace);
        int indexPieceWherePlaced = backgroundPiecesTransforms.IndexOf(backgroundPieceWherePlaced);

        isCorrect = indexPieceToPlace == indexPieceWherePlaced;

        PieceOriginalWorldTranformData placeToPutPiece = piecesOriginalTransforms[indexPieceWherePlaced];
        PuzzlePiecesMovementManager.instance.PlacePiece(pieceToPlace, placeToPutPiece, isCorrect);

        /*Debug.Log(indexPieceToPlace.ToString() + ' ' + indexPieceWherePlaced.ToString());*/
        if (isCorrect)
        {
            /*piecesPlacedCorrectly++;*/
            // for testing purposes
            piecesPlacedCorrectly = puzzlePiecesTransforms.Count;
            if (piecesPlacedCorrectly == puzzlePiecesTransforms.Count)
            {
                WinAction();
            }
        }
    }

    private void WinAction()
    {
        winMessage.SetActive(true);
    }
}
