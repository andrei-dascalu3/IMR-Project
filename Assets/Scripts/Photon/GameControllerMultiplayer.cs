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
        PlacePieceUnderOriginalParent(args.interactable.gameObject.transform);

        PhotonView piecePv = args.interactable.gameObject.GetComponent<PhotonView>();

        if (piecePv != null)
        {
            piecePv.TransferOwnership(PhotonNetwork.LocalPlayer);

            puzzleManagerMultiplayer.pv.RPC("SetPieceGrabable", RpcTarget.Others, false, puzzleManagerMultiplayer.puzzlePiecesTransforms.IndexOf(piecePv.transform));

            base.OnGrabEnter(args);
        }   
    }

    public void PlacePieceUnderOriginalParent(Transform piece)
    {
        piece.SetParent(PuzzleSetupManager.instance.puzzle);
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

    public override void LoseAction()
    {
        base.LoseAction();

        NetworkAvatar ownNetworkAvatar = PhotonRoomController.room.ownNetworkAvatar;
        ownNetworkAvatar.pv.RPC("BreakNetworkAvatar", RpcTarget.All);
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

        base.OnPieceCorrectPlace(indexPieceToPlace);

        //if(testWinConditionGame)
        //{
        //    piecesPlacedCorrectly = puzzleManagerMultiplayer.puzzlePiecesTransforms.Count;
        //}
        //else
        //{
        //    piecesPlacedCorrectly++;
        //}

        puzzleManagerMultiplayer.puzzlePiecesTransforms[indexPieceToPlace]
            .gameObject.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Off;

        puzzleManagerMultiplayer.puzzlePiecesTransforms[indexPieceToPlace]
            .gameObject.GetComponent<PhotonTransformView>().enabled = false;
    }

    public override void OnPieceIncorrectPlace(int indexPieceToPlace)
    {
        puzzleManagerMultiplayer.pv.RPC("SetPieceGrabable", RpcTarget.All, true, indexPieceToPlace);
        base.OnPieceIncorrectPlace(indexPieceToPlace);
    }

    [PunRPC]
    public void SyncCorrectPiecesPlaced(int indexPieceToPlace)
    {
        PieceTranformData placeToPutPiece = puzzleManagerMultiplayer.piecesOriginalTransforms[indexPieceToPlace];

        puzzleManagerMultiplayer.puzzlePiecesTransforms[indexPieceToPlace].GetComponent<PhotonView>().Synchronization = ViewSynchronization.Off;

        puzzleManagerMultiplayer.puzzlePiecesTransforms[indexPieceToPlace].GetComponent<PhotonTransformView>().enabled = false;

        Transform pieceToPlace = puzzleManagerMultiplayer.puzzlePiecesTransforms[indexPieceToPlace];

        puzzleManagerMultiplayer.PlacePiece(pieceToPlace, indexPieceToPlace, true);
        pieceToPlace.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        if (testWinConditionGame)
        {
            piecesPlacedCorrectly = puzzleManagerMultiplayer.puzzlePiecesTransforms.Count;
        }
        else
        {
            piecesPlacedCorrectly++;
        }
    }
}
