using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleCeilingPiecesCollisionScript : MonoBehaviour
{
    public PuzzlePiecesManager puzzleManager;
    public int piecesLayer;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DisableCeilingCollision", puzzleManager.timeUntilBreakPuzzleFinish + puzzleManager.timeExtraToDisablePiecesCollision);
    }

    // Update is called once per frame
    void DisableCeilingCollision()
    {
        Physics.IgnoreLayerCollision(gameObject.layer, piecesLayer);
    }
}
