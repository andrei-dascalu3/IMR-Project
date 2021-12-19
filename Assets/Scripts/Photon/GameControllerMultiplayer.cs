using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameControllerMultiplayer : GameController
{
    public PhotonView pv;

    public override void Awake()
    {
        base.Awake();

        pv = GetComponent<PhotonView>();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnGrabEnter(SelectEnterEventArgs args)
    {
        args.interactable.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        base.OnGrabEnter(args);
    }

    public override void OnGrabExit(SelectExitEventArgs args)
    {
        if (args.interactable.gameObject.GetComponent<PhotonView>().AmOwner)
        {
            base.OnGrabExit(args);
        }
    }

    public override void WinAction()
    {
        base.WinAction();
    }

    public override void TryPlacePiece(Transform pieceToPlace, Transform backgroundPieceWherePlaced)
    {
        if (pieceToPlace.GetComponent<PhotonView>().AmOwner)
            base.TryPlacePiece(pieceToPlace, backgroundPieceWherePlaced);
    }

    public override void OnPieceCorrectPlace(int indexPieceToPlace)
    {
        pv.RPC("SyncCorrectPiecesPlaced", RpcTarget.Others, indexPieceToPlace);

        //piecesPlacedCorrectly++;
        piecesPlacedCorrectly = puzzlePiecesTransforms.Count;

        puzzlePiecesTransforms[indexPieceToPlace].gameObject.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Off;

        puzzlePiecesTransforms[indexPieceToPlace].gameObject.GetComponent<PhotonTransformView>().enabled = false;
    }

    [PunRPC]
    public void SyncCorrectPiecesPlaced(int indexPieceToPlace)
    {
        PieceTranformData placeToPutPiece = piecesOriginalTransforms[indexPieceToPlace];

        puzzlePiecesTransforms[indexPieceToPlace].gameObject.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Off;

        puzzlePiecesTransforms[indexPieceToPlace].gameObject.GetComponent<PhotonTransformView>().enabled = false;

        Transform pieceToPlace = puzzlePiecesTransforms[indexPieceToPlace];

        puzzleMovementManager.PlacePiece(pieceToPlace, placeToPutPiece, true);
        pieceToPlace.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        //piecesPlacedCorrectly++;
        piecesPlacedCorrectly = puzzlePiecesTransforms.Count;
    }
}
