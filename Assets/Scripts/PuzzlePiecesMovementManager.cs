using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PuzzlePiecesMovementManager : MonoBehaviour
{
    private List<PuzzlePiece> puzzlePieces = new List<PuzzlePiece>();
    private List<Transform> placesForPiecesOnPlatform = new List<Transform>();

    public Transform placesForPiecesOnPlatformParent;

    public float timeUntilBreakPuzzle = 10;

    public void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            puzzlePieces.Add(transform.GetChild(i).GetComponent<PuzzlePiece>());
        }

        for (int i = 0; i < placesForPiecesOnPlatformParent.childCount; i++)
        {
            placesForPiecesOnPlatform.Add(placesForPiecesOnPlatformParent.GetChild(i));
        }
    }

    private void Start()
    {
        BreakPuzzle();
    }

    public void BreakPuzzle()
    {
        bool[] brokenPieces = new bool[puzzlePieces.Count];
        for(int i = 0; i < brokenPieces.Length; i++)
        {
            brokenPieces[i] = false;
        }

        int x;
        for(int i = 0; i < brokenPieces.Length; i++)
        {
            do{
                x = Random.Range(0, brokenPieces.Length);
            } while (brokenPieces[x] == true);
            brokenPieces[x] = true;
            int indexPlaceToLand = Random.Range(0, placesForPiecesOnPlatform.Count);
            puzzlePieces[x].SetPlaceToLand(placesForPiecesOnPlatform[indexPlaceToLand]);
            StartCoroutine(puzzlePieces[x].BreakPiece());
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
