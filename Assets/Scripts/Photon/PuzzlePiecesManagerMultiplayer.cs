using Photon.Pun;
using UnityEngine;


public class PuzzlePiecesManagerMultiplayer : PuzzlePiecesManager
{
    public PhotonView pv;

    public int[] orderToBreakPieces;
    public int[] piecesLandingPlaces;

    public float timeExtraToEnablePhotonTransforms = 5;

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
    }

    public override void Start()
    {
        for (int i = 0; i < puzzlePiecesTransforms.Count; i++)
        {
            PhotonView piecePv = puzzlePiecesTransforms[i].gameObject.GetComponent<PhotonView>();
            piecePv.observableSearch = PhotonView.ObservableSearch.Manual;
            piecePv.ObservedComponents.Clear();

            pv.ObservedComponents.Add(piecePv.GetComponent<PhotonTransformView>());
        }

        if (PhotonNetwork.IsMasterClient)
        {
            base.Start();
        }
        else
        {
            SavePiecesOriginalLocations();
            Invoke("DisablePiecesCollision", timeExtraToEnablePhotonTransforms + timeUntilBreakPuzzleFinish);
        }
        Invoke("EnablePhotonTransforms", timeExtraToEnablePhotonTransforms + timeUntilBreakPuzzleFinish);
    }

    private void InitStartAnimationIndexes()
    {
        orderToBreakPieces = new int[puzzlePiecesTransforms.Count];
        piecesLandingPlaces = new int[puzzlePiecesTransforms.Count];

        bool[] brokenPieces = new bool[puzzlePiecesTransforms.Count];
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

        for (int i = 0; i < puzzlePiecesTransforms.Count; i++)
        {
            PhotonView piecePv = puzzlePiecesTransforms[i].gameObject.GetComponent<PhotonView>();

            piecePv.ObservedComponents.Add(piecePv.GetComponent<PhotonTransformView>());
            piecePv.Synchronization = ViewSynchronization.ReliableDeltaCompressed;
            piecePv.OwnershipTransfer = OwnershipOption.Takeover;
        }

        Debug.Log("Enables pieces PhotonView s");
    }

    [PunRPC]
    public override void SetPieceGrabable(bool b, int pieceIndex)
    {
        base.SetPieceGrabable(b, pieceIndex);
    }
}
