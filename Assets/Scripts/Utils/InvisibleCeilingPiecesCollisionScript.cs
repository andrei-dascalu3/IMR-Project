using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleCeilingPiecesCollisionScript : MonoBehaviour
{
    public PuzzlePiecesManager puzzleManager;

    void Start()
    {
        Invoke(nameof(DisableCeilingCollision), puzzleManager.timeUntilBreakPuzzleFinish + puzzleManager.timeExtraToDisablePiecesCollision);
    }

    void DisableCeilingCollision()
    {
        Physics.IgnoreLayerCollision(LayerDataObject.instance.ceilingLayer, LayerDataObject.instance.puzzlePieceLayer);
    }
}
