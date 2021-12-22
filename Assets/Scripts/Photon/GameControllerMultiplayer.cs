using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameControllerMultiplayer : GameController
{
    public PhotonView pv;

    private PuzzlePiecesManagerMultiplayer puzzleManagerMultiplayer;

    public void Awake()
    {
        pv = GetComponent<PhotonView>();

        puzzleManagerMultiplayer = puzzleManager as PuzzlePiecesManagerMultiplayer;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnGrabEnter(SelectEnterEventArgs args)
    {
        PhotonView piecePv = args.interactable.gameObject.GetComponent<PhotonView>();

        if (piecePv != null)
        {
            piecePv.TransferOwnership(PhotonNetwork.LocalPlayer);

            puzzleManagerMultiplayer.pv.RPC("SetPieceGrabable", RpcTarget.Others, false, puzzleManagerMultiplayer.puzzlePiecesTransforms.IndexOf(piecePv.transform));

            base.OnGrabEnter(args);
        }   
    }

    public override void OnGrabExit(SelectExitEventArgs args)
    {
        PhotonView piecePv = args.interactable.gameObject.GetComponent<PhotonView>();
        if (piecePv != null && piecePv.AmOwner)
        {
            puzzleManagerMultiplayer.pv.RPC("SetPieceGrabable", RpcTarget.Others, true, puzzleManagerMultiplayer.puzzlePiecesTransforms.IndexOf(piecePv.transform));
            base.OnGrabExit(args);
        }
    }

    public override void WinAction()
    {
        base.WinAction();
    }

    public override void TryPlacePiece(Transform pieceToPlace, Transform backgroundPieceWherePlaced)
    {
        PhotonView piecePv = pieceToPlace.GetComponent<PhotonView>();
        if (piecePv.AmOwner)
        {
            puzzleManagerMultiplayer.pv.RPC("SetPieceGrabable", RpcTarget.Others, false, puzzleManagerMultiplayer.puzzlePiecesTransforms.IndexOf(piecePv.transform));

            base.TryPlacePiece(pieceToPlace, backgroundPieceWherePlaced);
        }      
    }

    public override void OnPieceCorrectPlace(int indexPieceToPlace)
    {
        pv.RPC("SyncCorrectPiecesPlaced", RpcTarget.Others, indexPieceToPlace);

        //piecesPlacedCorrectly++;
        piecesPlacedCorrectly = puzzleManagerMultiplayer.puzzlePiecesTransforms.Count;

        puzzleManagerMultiplayer.puzzlePiecesTransforms[indexPieceToPlace]
            .gameObject.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Off;

        puzzleManagerMultiplayer.puzzlePiecesTransforms[indexPieceToPlace]
            .gameObject.GetComponent<PhotonTransformView>().enabled = false;
    }

    public override void OnPieceIncorrectPlace(int indexPieceToPlace)
    {
        //TODO, sa fie un delay??? sau sa fie grabable doar din mom in care atinge pamantu???
        puzzleManagerMultiplayer.pv.RPC("SetPieceGrabable", RpcTarget.All, true, indexPieceToPlace);
    }

    [PunRPC]
    public void SyncCorrectPiecesPlaced(int indexPieceToPlace)
    {
        PieceTranformData placeToPutPiece = puzzleManagerMultiplayer.piecesOriginalTransforms[indexPieceToPlace];

        puzzleManagerMultiplayer.puzzlePiecesTransforms[indexPieceToPlace].GetComponent<PhotonView>().Synchronization = ViewSynchronization.Off;

        puzzleManagerMultiplayer.puzzlePiecesTransforms[indexPieceToPlace].GetComponent<PhotonTransformView>().enabled = false;

        Transform pieceToPlace = puzzleManagerMultiplayer.puzzlePiecesTransforms[indexPieceToPlace];

        //puzzleManager.PlacePiece(pieceToPlace, placeToPutPiece, true);
        puzzleManagerMultiplayer.PlacePiece(pieceToPlace, indexPieceToPlace, true);
        pieceToPlace.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        //piecesPlacedCorrectly++;
        piecesPlacedCorrectly = puzzleManagerMultiplayer.puzzlePiecesTransforms.Count;
    }
}
