using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PuzzlePiecesMovementManager : MonoBehaviour
{
    public static PuzzlePiecesMovementManager instance;

    private List<PuzzlePiece> puzzlePieces = new List<PuzzlePiece>();
    private List<Transform> placesForPiecesOnPlatform = new List<Transform>();

    public Transform placesForPiecesOnPlatformParent;

    public float timeUntilBreakPuzzle = 10;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                PuzzlePiecesMovementManager.instance = this;
                GameObject.Destroy(instance.gameObject);
            }
        }

        CreatePiecesAndLandingsLists();
    }
    private void Start()
    {
        if (PhotonRoomController.room != null && !PhotonNetwork.IsMasterClient)
        {
            return;
        }
        BreakPuzzle();

    }

    private void CreatePiecesAndLandingsLists()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            puzzlePieces.Add(transform.GetChild(i).GetComponent<PuzzlePiece>());
        }

        for (int i = 0; i < placesForPiecesOnPlatformParent.childCount; i++)
        {
            placesForPiecesOnPlatform.Add(placesForPiecesOnPlatformParent.GetChild(i));
        }
    }


    public void BreakPuzzle()
    {
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
                x = Random.Range(0, brokenPieces.Length);
            } while (brokenPieces[x] == true);
            brokenPieces[x] = true;
            int indexPlaceToLand = Random.Range(0, placesForPiecesOnPlatform.Count);
            //puzzlePieces[x].SetPlaceToLand(placesForPiecesOnPlatform[indexPlaceToLand].position);
            StartCoroutine(puzzlePieces[x].BreakPiece(placesForPiecesOnPlatform[indexPlaceToLand].position));
        }
    }


    public void PlacePiece(Transform pieceToPlace, PieceOriginalWorldTranformData destinationTransform, bool placedCorrectly)
    {
        PuzzlePiece puzzlePiece = pieceToPlace.gameObject.GetComponent<PuzzlePiece>();
        int indexPlaceToLand = Random.Range(0, placesForPiecesOnPlatform.Count);
        StartCoroutine(puzzlePiece.PlaceOnPuzzle(destinationTransform, placedCorrectly, placesForPiecesOnPlatform[indexPlaceToLand].position));
    }
}
