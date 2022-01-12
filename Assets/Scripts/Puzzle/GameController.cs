using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameController : MonoBehaviour
{
    public PuzzlePiecesManager puzzleManager;

    [SerializeField]
    protected Transform currentlyHoverdBackroundPiece;
    [SerializeField]
    protected Transform currentlyHeldPiece;

    public XRRayInteractor leftHand;
    public XRRayInteractor rightHand;

    public GameObject winMessage;
    public GameObject loseMessage;
    public GameObject soundManager;

    protected int piecesPlacedCorrectly = 0;

    public virtual void Start()
    {
        SubscribeMethodsToXrEvents();
        SoundManagerScript.PlaySound("shockwave");
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
            puzzleManager.SetPieceGrabable(true, currentlyHeldPiece);
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
        puzzleManager.SetPieceGrabable(true, indexPieceToPlace);
        SoundManagerScript.PlaySound("wrongPlace");
    }

    public virtual void OnPieceCorrectPlace(int indexPieceToPlace)
    {
        /*piecesPlacedCorrectly++;*/
        // for testing purposes
        piecesPlacedCorrectly = puzzleManager.puzzlePiecesTransforms.Count;

        if (piecesPlacedCorrectly == puzzleManager.puzzlePiecesTransforms.Count)
        {
            WinAction();
        }
        // sound effect
        SoundManagerScript.PlaySound("rightPlace");
    }

    public virtual void WinAction()
    {
        winMessage.SetActive(true);
    }

    public virtual void LoseAction()
    {
        loseMessage.SetActive(true);
        AudioClip loseClip = Resources.Load<AudioClip>("Audio/LoseTrack");
        AudioSource audioSrc = soundManager.GetComponent<AudioSource>();
        audioSrc.PlayOneShot(loseClip);
    }
}
