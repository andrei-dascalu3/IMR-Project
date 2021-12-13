using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PuzzlePiecesMovementManager : MonoBehaviour
{
    //public static PuzzlePiecesMovementManager instance;

    protected List<PuzzlePiece> puzzlePieces = new List<PuzzlePiece>();
    protected List<Transform> placesForPiecesOnPlatform = new List<Transform>();

    public Transform placesForPiecesOnPlatformParent;

    public float timeUntilBreakPuzzle = 3;

    public virtual void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    if (instance != this)
        //    {
        //        PuzzlePiecesMovementManager.instance = this;
        //        GameObject.Destroy(instance.gameObject);
        //    }
        //}

        CreatePiecesAndLandingsLists();
    }
    public virtual void Start()
    {
        Invoke("BreakPuzzle", timeUntilBreakPuzzle);
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
                x = GetIndexOfPieceToBreak();
            } while (brokenPieces[x] == true);
            brokenPieces[x] = true;

            int indexPlaceToLand = GetIndexOfPlaceToLand();
            Vector3 placeToLand = placesForPiecesOnPlatform[indexPlaceToLand].position;
            StartCoroutine(puzzlePieces[x].BreakPiece(placeToLand));
        }
    }


    public virtual int GetIndexOfPieceToBreak()
    {
        return Random.Range(0, puzzlePieces.Count);
    }

    public virtual int GetIndexOfPlaceToLand()
    {
        return Random.Range(0, placesForPiecesOnPlatform.Count);
    }


    public void PlacePiece(Transform pieceToPlace, PieceTranformData destinationTransform, bool placedCorrectly)
    {
        PuzzlePiece puzzlePiece = pieceToPlace.gameObject.GetComponent<PuzzlePiece>();
        int indexPlaceToLand = GetIndexOfPlaceToLand();
        Vector3 placeToLandIfFail = placesForPiecesOnPlatform[indexPlaceToLand].position;
        StartCoroutine(puzzlePiece.PlaceOnPuzzle(destinationTransform, placedCorrectly, placeToLandIfFail));
    }
}
