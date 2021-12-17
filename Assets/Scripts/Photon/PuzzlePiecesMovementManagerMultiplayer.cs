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

    public float timeExtraToEnablePhotonTransforms = 1f;
    public float timeUntilStartAnimationFinishes;

    public int piecesBroken = 0;

    float[] positions;
    float[] rotations;
    float[] scales;

    public override void Awake()
    {
        pv = GetComponent<PhotonView>();

        base.Awake();

        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            puzzlePieces[i].GetComponent<PhotonView>().Synchronization = ViewSynchronization.Off;
            puzzlePieces[i].GetComponent<PhotonView>().OwnershipTransfer = OwnershipOption.Takeover;
        }

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

        base.Start();
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    positions = new float[puzzlePieces.Count * 3];
        //    rotations = new float[puzzlePieces.Count * 3];
        //    scales = new float[puzzlePieces.Count * 3];


        //}
        //else
        //{
        //    //Invoke("RemovePiecesConstraints", timeUntilStartAnimationFinishes + timeExtraToEnablePhotonTransforms);
        //}


        Invoke("EnablePhotonTransforms", timeExtraToEnablePhotonTransforms + timeUntilStartAnimationFinishes);
        Invoke("DisablePiecesCollision", timeExtraToEnablePhotonTransforms + timeUntilStartAnimationFinishes);
    }

    //public void RemovePiecesConstraints()
    //{
    //    for (int i = 0; i < puzzlePieces.Count; i++)
    //    {
    //        puzzlePieces[i].ownRigidBody.constraints = RigidbodyConstraints.None;
    //    }
    //}

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
        //pv.Synchronization = ViewSynchronization.UnreliableOnChange;
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            puzzlePieces[i].GetComponent<PhotonView>().Synchronization = ViewSynchronization.UnreliableOnChange;
        }
    }

    [PunRPC]
    public void DisablePiecesCollision()
    {
        Physics.IgnoreLayerCollision(6, 6);
    }

    //[PunRPC]
    //public void UpdatePuzzlePiecesTransforms(float[] positions, float[] rotations, float[] scales)
    //{
    //    //for (int i = 0; i < puzzlePieces.Count; i++)
    //    //{
    //    //    //puzzlePieces[i].transform.position = tranformData.position;
    //    //    //puzzlePieces[i].transform.rotation = tranformData.rotation;
    //    //    //puzzlePieces[i].transform.localScale = tranformData.scale;
    //    //}
    //    for (int i = 0; i < puzzlePieces.Count; i++)
    //    {
    //        puzzlePieces[i].transform.position = new Vector3(positions[i], positions[i + puzzlePieces.Count], positions[i + 2 * puzzlePieces.Count]);
    //        puzzlePieces[i].transform.rotation = new Quaternion(rotations[i], rotations[i + puzzlePieces.Count], rotations[i + 2 * puzzlePieces.Count], 0);
    //        puzzlePieces[i].transform.localScale = new Vector3(scales[i], scales[i + puzzlePieces.Count], scales[i + 2 * puzzlePieces.Count]);
    //    }
    //}

    private void Update()
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    for (int i = 0; i < puzzlePieces.Count; i++)
        //    {
        //        //piecesTranformDatas[i].position = puzzlePieces[i].transform.position;
        //        //piecesTranformDatas[i].rotation = puzzlePieces[i].transform.rotation;
        //        //piecesTranformDatas[i].scale = puzzlePieces[i].transform.localScale;
        //        positions[i] = puzzlePieces[i].transform.position.x;
        //        positions[i + puzzlePieces.Count] = puzzlePieces[i].transform.position.y;
        //        positions[i + 2 * puzzlePieces.Count] = puzzlePieces[i].transform.position.z;

        //        rotations[i] = puzzlePieces[i].transform.rotation.x;
        //        rotations[i + puzzlePieces.Count] = puzzlePieces[i].transform.rotation.y;
        //        rotations[i + 2 * puzzlePieces.Count] = puzzlePieces[i].transform.rotation.z;

        //        scales[i] = puzzlePieces[i].transform.localScale.x;
        //        scales[i + puzzlePieces.Count] = puzzlePieces[i].transform.localScale.y;
        //        scales[i + 2 * puzzlePieces.Count] = puzzlePieces[i].transform.localScale.z;
        //    }

        //    pv.RPC("UpdatePuzzlePiecesTransforms", RpcTarget.Others, positions, rotations, scales);
        //}
    }
}
