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

    public PuzzleSetupManager setupManager;

    public float timeUntilBreakPuzzle = 3;
    public float timeUntilBreakPuzzleFinish;
    public float timeExtraToDisablePiecesCollision = 5;

    public virtual void Awake()
    {
        CreatePiecesAndLandingsLists();

        timeUntilBreakPuzzleFinish = PuzzlePiece.durationForPieceBreak
                                + PuzzlePiece.shakeSegments * PuzzlePiece.shakeTimePerSegment
                                + timeUntilBreakPuzzle;
    }

    public virtual void Start()
    {
        SavePiecesOriginalLocations();

        Invoke(nameof(BreakPuzzle), timeUntilBreakPuzzle);
        Invoke(nameof(DisablePiecesCollision), timeExtraToDisablePiecesCollision + timeUntilBreakPuzzleFinish);
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

    public void CreatePiecesAndLandingsLists()
    {
        puzzlePiecesTransforms = new List<Transform>(transform.GetComponentsInChildren<Transform>());
        puzzlePiecesTransforms.RemoveAt(0);

        backgroundPiecesTransforms = new List<Transform>(setupManager.backgroundPuzzle.GetComponentsInChildren<Transform>());
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
        Debug.Log(indexPieceDestination);
        PieceTranformData placeToPutPiece = piecesOriginalTransforms[indexPieceDestination];

        PuzzlePiece puzzlePiece = pieceToPlace.gameObject.GetComponent<PuzzlePiece>();

        //int indexPlaceToLand = GetIndexOfPlaceToLand();
        int indexPlaceToLand = Random.Range(0, placesForPiecesOnPlatform.Count);
        Vector3 placeToLandIfFail = placesForPiecesOnPlatform[indexPlaceToLand].position;

        StartCoroutine(puzzlePiece.PlaceOnPuzzle(placeToPutPiece, placedCorrectly, placeToLandIfFail));
    }

    public void DisablePiecesCollision()
    {
        int grabablePieceLayer = LayerDataObject.instance.puzzlePieceLayer;
        int ungrabablePieceLayer = LayerDataObject.instance.ungrabablePuzzlePieceLayer;

        Physics.IgnoreLayerCollision(grabablePieceLayer, grabablePieceLayer);
        Physics.IgnoreLayerCollision(grabablePieceLayer, ungrabablePieceLayer);
        Physics.IgnoreLayerCollision(ungrabablePieceLayer, grabablePieceLayer);
    }

    public void SetPieceGrabable(bool b, Transform piece)
    {
        PuzzlePiece puzzlePiece = piece.GetComponent<PuzzlePiece>();
        if (b)
        {
            puzzlePiece.SetSelfLayer(LayerDataObject.instance.puzzlePieceLayer);
        }
        else
        {
            puzzlePiece.SetSelfLayer(LayerDataObject.instance.ungrabablePuzzlePieceLayer);
        }
    }

    public virtual void SetPieceGrabable(bool b, int pieceIndex)
    {
        PuzzlePiece puzzlePiece = puzzlePiecesTransforms[pieceIndex].GetComponent<PuzzlePiece>();
        if (b)
        {
            puzzlePiecesTransforms[pieceIndex].GetComponent<Rigidbody>().isKinematic = false;
            puzzlePiece.SetSelfLayer(LayerDataObject.instance.puzzlePieceLayer);
        }
        else
        {
            puzzlePiecesTransforms[pieceIndex].GetComponent<Rigidbody>().isKinematic = true;
            puzzlePiece.SetSelfLayer(LayerDataObject.instance.ungrabablePuzzlePieceLayer);
        }
    }
}
