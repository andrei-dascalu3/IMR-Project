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
        PhotonView piecePv = args.interactable.gameObject.GetComponent<PhotonView>();
        piecePv.TransferOwnership(PhotonNetwork.LocalPlayer);

        pv.RPC("SetSyncPiece", RpcTarget.All, true, puzzlePiecesTransforms.IndexOf(piecePv.transform));

        base.OnGrabEnter(args);
    }

    [PunRPC]
    public void SetSyncPiece(bool b, int pieceIndex)
    {
        PhotonView piecePv = puzzlePiecesTransforms[pieceIndex].GetComponent<PhotonView>();
        if (b)
        {
            piecePv.Synchronization = ViewSynchronization.ReliableDeltaCompressed;
        }
        else
        {
            piecePv.Synchronization = ViewSynchronization.Off;
        }
    }

    public override void OnGrabExit(SelectExitEventArgs args)
    {
        PhotonView piecePv = args.interactable.gameObject.GetComponent<PhotonView>();
        if (piecePv.AmOwner)
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
        PhotonView piecePv = pieceToPlace.GetComponent<PhotonView>();
        if (piecePv.AmOwner)
        {
            //pv.RPC("SetSyncPiece", RpcTarget.All, false, puzzlePiecesTransforms.IndexOf(piecePv.transform));

            pv.RPC("SetPieceGrabable", RpcTarget.Others, false, puzzlePiecesTransforms.IndexOf(piecePv.transform));

            base.TryPlacePiece(pieceToPlace, backgroundPieceWherePlaced);
        }      
    }

    [PunRPC]
    public override void SetPieceGrabable(bool b, int pieceIndex)
    {
        base.SetPieceGrabable(b, pieceIndex);
    }

    public override void OnPieceCorrectPlace(int indexPieceToPlace)
    {
        pv.RPC("SetSyncPiece", RpcTarget.All, false, 
            puzzlePiecesTransforms.IndexOf(puzzlePiecesTransforms[indexPieceToPlace].transform));

        pv.RPC("SyncCorrectPiecesPlaced", RpcTarget.Others, indexPieceToPlace);

        //piecesPlacedCorrectly++;
        piecesPlacedCorrectly = puzzlePiecesTransforms.Count;

        puzzlePiecesTransforms[indexPieceToPlace].gameObject.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Off;

        puzzlePiecesTransforms[indexPieceToPlace].gameObject.GetComponent<PhotonTransformView>().enabled = false;
    }

    public override void OnPieceIncorrectPlace(int indexPieceToPlace)
    {
        //TODO, sa fie un delay??? sau sa fie grabable doar din mom in care atinge pamantu???
        //base.OnPieceIncorrectPlace(indexPieceToPlace);
        pv.RPC("SetPieceGrabable", RpcTarget.All, true, indexPieceToPlace);
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
