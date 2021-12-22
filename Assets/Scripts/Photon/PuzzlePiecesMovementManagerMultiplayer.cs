using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PuzzlePiecesMovementManagerMultiplayer : PuzzlePiecesMovementManager
{
    public PhotonView pv;

    public int[] orderToBreakPieces;
    public int[] piecesLandingPlaces;

    public float timeExtraToEnablePhotonTransforms = 5;
    public float timeUntilStartAnimationFinishes;

    public int piecesBroken = 0;

    public override void Awake()
    {
        pv = GetComponent<PhotonView>();
        pv.Synchronization = ViewSynchronization.ReliableDeltaCompressed;
        pv.observableSearch = PhotonView.ObservableSearch.Manual;

        base.Awake();

        if (PhotonNetwork.IsMasterClient)
        {
            InitStartAnimationIndexes();
        }

        timeUntilStartAnimationFinishes = PuzzlePiece.durationForPieceBreak
                                        + PuzzlePiece.shakeSegments * PuzzlePiece.shakeTimePerSegment
                                        + timeUntilBreakPuzzle;
    }

    public override void Start()
    {
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            PhotonView piecePv = puzzlePieces[i].gameObject.GetComponent<PhotonView>();
            piecePv.observableSearch = PhotonView.ObservableSearch.Manual;
            piecePv.ObservedComponents.Clear();

            pv.ObservedComponents.Add(piecePv.GetComponent<PhotonTransformView>());
        }

        if (PhotonNetwork.IsMasterClient)
        {
            base.Start();
        }

        Invoke("EnablePhotonTransforms", timeExtraToEnablePhotonTransforms + timeUntilStartAnimationFinishes);
        Invoke("DisablePiecesCollision", timeExtraToEnablePhotonTransforms + timeUntilStartAnimationFinishes);
    }

    private void InitStartAnimationIndexes()
    {
        orderToBreakPieces = new int[puzzlePieces.Count];
        piecesLandingPlaces = new int[puzzlePieces.Count];

        bool[] brokenPieces = new bool[puzzlePieces.Count];
        for (int i = 0; i < brokenPieces.Length; i++)
        {
            brokenPieces[i] = false;
        }

        int x;
        for (int i = 0; i < brokenPieces.Length; i++)
        {
            do
            {
                x = base.GetIndexOfPieceToBreak();
            } while (brokenPieces[x] == true);
            brokenPieces[x] = true;
            orderToBreakPieces[i] = x;

            piecesLandingPlaces[i] = base.GetIndexOfPlaceToLand();
        }

        pv.RPC("SyncInitStartAnimationIndexes", RpcTarget.Others, orderToBreakPieces, piecesLandingPlaces);
    }

    public override int GetIndexOfPieceToBreak()
    {
        int x = orderToBreakPieces[piecesBroken];
        return x;
    }

    public override int GetIndexOfPlaceToLand()
    {
        int x = piecesLandingPlaces[piecesBroken];
        piecesBroken++;
        return x;
    }

    [PunRPC]
    public void SyncInitStartAnimationIndexes(int[] orderToBreakPieces, int[] piecesLandingPlaces)
    {
        this.orderToBreakPieces = orderToBreakPieces;
        this.piecesLandingPlaces = piecesLandingPlaces;
    }

    [PunRPC]
    public void EnablePhotonTransforms()
    {
        pv.ObservedComponents.Clear();

        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            //PhotonView piecePv = puzzlePieces[i].gameObject.AddComponent<PhotonView>();
            PhotonView piecePv = puzzlePieces[i].gameObject.GetComponent<PhotonView>();
            piecePv.ObservedComponents.Add(piecePv.GetComponent<PhotonTransformView>());
            //piecePv.Synchronization = ViewSynchronization.Off;
            piecePv.Synchronization = ViewSynchronization.ReliableDeltaCompressed;
            piecePv.OwnershipTransfer = OwnershipOption.Takeover;
            //piecePv.ViewID = i + 2;
            //piecePv.observableSearch = PhotonView.ObservableSearch.AutoFindAll;
            //if (!PhotonNetwork.IsMasterClient)
              //  piecePv.TransferOwnership(PhotonNetwork.MasterClient);
        }

        Debug.Log("Enables pieces PhotonView s");
    }

    [PunRPC]
    public void DisablePiecesCollision()
    {
        Physics.IgnoreLayerCollision(6, 6);
    }
}
