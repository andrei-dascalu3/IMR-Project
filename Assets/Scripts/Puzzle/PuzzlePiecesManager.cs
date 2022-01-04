using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct PieceTranformData
{
    public Vector3 position;
    public Quaternion rotation;
}

public class PuzzlePiecesManager : MonoBehaviour
{
    public List<PieceTranformData> piecesOriginalTransforms;

    //protected List<PuzzlePiece> puzzlePieces = new List<PuzzlePiece>();

    public List<Transform> puzzlePiecesTransforms;
    public List<Transform> backgroundPiecesTransforms;

    public Transform placesForPiecesOnPlatformParent;
    protected List<Transform> placesForPiecesOnPlatform = new List<Transform>();

    public float timeUntilBreakPuzzle = 3;
    public float timeUntilBreakPuzzleFinish;
    public float timeExtraToDisablePiecesCollision = 5;

    public virtual void Awake()
    {
        CreatePiecesAndLandingsLists();

        timeUntilBreakPuzzleFinish = PuzzlePiece.durationForPieceBreak
                                        + PuzzlePiece.shakeSegments * PuzzlePiece.shakeTimePerSegment
                                        + timeUntilBreakPuzzle;

        SavePiecesOriginalLocations();
    }
    public virtual void Start()
    {
        Invoke("BreakPuzzle", timeUntilBreakPuzzle);
        Invoke("DisablePiecesCollision", timeExtraToDisablePiecesCollision + timeUntilBreakPuzzleFinish);
    }

    public void SavePiecesOriginalLocations()
    {
        piecesOriginalTransforms = new List<PieceTranformData>();
        for (int i = 0; i < puzzlePiecesTransforms.Count; i++)
        {
            PieceTranformData ogTransform = new PieceTranformData();
            ogTransform.position = puzzlePiecesTransforms[i].transform.position;
            ogTransform.rotation = puzzlePiecesTransforms[i].transform.rotation;
            piecesOriginalTransforms.Add(ogTransform);
        }
    }

    private void CreatePiecesAndLandingsLists()
    {
        puzzlePiecesTransforms = new List<Transform>
            (transform.GetComponentsInChildren<Transform>());
        puzzlePiecesTransforms.RemoveAt(0);

        backgroundPiecesTransforms = new List<Transform>
            (PuzzleSetupManager.instance.backgroundPuzzle.GetComponentsInChildren<Transform>());
        backgroundPiecesTransforms.RemoveAt(0);

        //puzzlePieces = new List<PuzzlePiece>(transform.GetComponentsInChildren<PuzzlePiece>());
        placesForPiecesOnPlatform = new List<Transform>
            (placesForPiecesOnPlatformParent.GetComponentsInChildren<Transform>());
        placesForPiecesOnPlatform.RemoveAt(0);
    }

    public void BreakPuzzle()
    {
        //bool[] brokenPieces = new bool[puzzlePieces.Count];
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
                x = GetIndexOfPieceToBreak();
            } while (brokenPieces[x] == true);
            brokenPieces[x] = true;

            int indexPlaceToLand = GetIndexOfPlaceToLand();
            Vector3 placeToLand = placesForPiecesOnPlatform[indexPlaceToLand].position;

            PuzzlePiece puzzlePiece = puzzlePiecesTransforms[x].GetComponent<PuzzlePiece>();
            StartCoroutine(puzzlePiece.BreakPiece(placeToLand));
        }
    }

    public virtual int GetIndexOfPieceToBreak()
    {
        return Random.Range(0, puzzlePiecesTransforms.Count);
    }

    public virtual int GetIndexOfPlaceToLand()
    {
        return Random.Range(0, placesForPiecesOnPlatform.Count);
    }


    //public void PlacePiece(Transform pieceToPlace, PieceTranformData destinationTransform, bool placedCorrectly)
    public void PlacePiece(Transform pieceToPlace, int indexPieceDestination, bool placedCorrectly)
    {
        PieceTranformData placeToPutPiece = piecesOriginalTransforms[indexPieceDestination];

        PuzzlePiece puzzlePiece = pieceToPlace.gameObject.GetComponent<PuzzlePiece>();

        //int indexPlaceToLand = GetIndexOfPlaceToLand();
        int indexPlaceToLand = Random.Range(0, placesForPiecesOnPlatform.Count);
        Vector3 placeToLandIfFail = placesForPiecesOnPlatform[indexPlaceToLand].position;

        StartCoroutine(puzzlePiece.PlaceOnPuzzle(placeToPutPiece, placedCorrectly, placeToLandIfFail));
    }

    public void DisablePiecesCollision()
    {
        Physics.IgnoreLayerCollision(6, 6);
        Physics.IgnoreLayerCollision(6, 7);
        Physics.IgnoreLayerCollision(7, 6);
    }

    public void SetPieceGrabable(bool b, Transform piece)
    {
        if (b)
        {
            piece.gameObject.layer = 6;
        }
        else
        {
            piece.gameObject.layer = 7;
        }
    }

    public virtual void SetPieceGrabable(bool b, int pieceIndex)
    {
        if (b)
        {
            puzzlePiecesTransforms[pieceIndex].gameObject.layer = 6;
        }
        else
        {
            puzzlePiecesTransforms[pieceIndex].gameObject.layer = 7;
        }
    }
}
