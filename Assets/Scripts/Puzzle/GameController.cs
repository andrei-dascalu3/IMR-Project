using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameController : MonoBehaviour
{
    public PuzzlePiecesManager puzzleManager;

    [SerializeField]
    protected Transform currentlyHoverdBackroundPiece;
    [SerializeField]
    protected Transform currentlyHeldPiece;

    protected CanvasElementsObjectScript canvasElements;

    public GameObject soundManager;

    protected int piecesPlacedCorrectly = 0;

    public bool testWinConditionGame;

    public virtual void Start()
    {       
        EnableTimer();

        SubscribeMethodsToXrEvents();

        SoundManagerScript.PlaySound("shockwave");
    }

    private void EnableTimer()
    {
        canvasElements = CanvasElementsObjectScript.playerCanvas;
        canvasElements.timerCanvas.SetActive(true);
    }

    private void SubscribeMethodsToXrEvents()
    {
        XRRayInteractor leftController = BrokenWorldPlayer.player.leftController;
        XRRayInteractor rightController = BrokenWorldPlayer.player.rightController;

        leftController.selectEntered.AddListener(OnGrabEnter);
        leftController.selectExited.AddListener(OnGrabExit);

        rightController.selectEntered.AddListener(OnGrabEnter);
        rightController.selectExited.AddListener(OnGrabExit);

        leftController.hoverEntered.AddListener(OnHoverEnter);
        leftController.hoverExited.AddListener(OnHoverExit);

        rightController.hoverEntered.AddListener(OnHoverEnter);
        rightController.hoverExited.AddListener(OnHoverExit);
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

        //reset piece parent to puzzle parent
        //currentlyHeldPiece.parent = PuzzleSetupManager.instance.puzzle.transform;

        puzzleManager.SetPieceGrabable(false, currentlyHeldPiece);

        // sound effect
        SoundManagerScript.PlaySound("grab");
    }

    public virtual void OnGrabExit(SelectExitEventArgs args)
    {
        if (currentlyHoverdBackroundPiece == null)
        {
            puzzleManager.SetPieceGrabable(true, currentlyHeldPiece);
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

        int indexPieceToPlace = puzzleManager.puzzlePiecesTransforms.IndexOf(pieceToPlace);
        int indexPieceDestination = puzzleManager.backgroundPiecesTransforms.IndexOf(backgroundPieceWherePlaced);

        isCorrect = (indexPieceToPlace == indexPieceDestination);

        puzzleManager.PlacePiece(pieceToPlace, indexPieceDestination, isCorrect);

        if (isCorrect)
        {
            OnPieceCorrectPlace(indexPieceToPlace);   
        }
        else
        {
            OnPieceIncorrectPlace(indexPieceToPlace);    
        }
    }

    public virtual void OnPieceIncorrectPlace(int indexPieceToPlace)
    {
        SoundManagerScript.PlaySound("wrongPlace");
    }

    public virtual void OnPieceCorrectPlace(int indexPieceToPlace)
    { 
        if(testWinConditionGame)
        {
            // for testing purposes
            piecesPlacedCorrectly = puzzleManager.puzzlePiecesTransforms.Count;
        }
        else
        {
            piecesPlacedCorrectly++;
        }   

        if (piecesPlacedCorrectly == puzzleManager.puzzlePiecesTransforms.Count)
        {
            WinAction();
        }
        // sound effect
        SoundManagerScript.PlaySound("rightPlace");
    }

    public virtual void WinAction()
    {
        canvasElements.winText.gameObject.SetActive(true);
    }

    public virtual void LoseAction()
    {
        canvasElements.loseText.gameObject.SetActive(true);
        AudioClip loseClip = Resources.Load<AudioClip>("Audio/LoseTrack");
        AudioSource audioSrc = soundManager.GetComponent<AudioSource>();
        audioSrc.PlayOneShot(loseClip);

        BrokenWorldPlayer.player.BreakCharacter();
    }
}
