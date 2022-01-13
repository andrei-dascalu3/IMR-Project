using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzlePiece : MonoBehaviour
{
    public Rigidbody ownRigidBody;

    public const float durationForPieceBreak = 3; //2
    public float smallPieceScaleTarget = 0.1f;

    public const float shakeSegments = 10;
    public const float shakeTimePerSegment = 2 / shakeSegments;
    public float shakeStrengthMultiplier = 3;
    public float shakeStrengthStart = 3;
    public int shakeVibrato = 5;

    public bool placedCorrectly = false;

    private void Awake()
    {
        ownRigidBody = GetComponent<Rigidbody>();
    }

    public IEnumerator BreakPiece(Vector3 placeToLand)
    {
        SetSelfLayer(LayerDataObject.instance.ungrabablePuzzlePieceLayer);

        ownRigidBody.constraints = RigidbodyConstraints.FreezePosition;

        float timer = 0f;
        while (timer < shakeTimePerSegment * shakeSegments)
        {
            transform.DOShakeRotation(shakeTimePerSegment, shakeStrengthStart + shakeStrengthMultiplier * timer * (5 / (shakeTimePerSegment * shakeSegments)), shakeVibrato, 3, false);

            timer += shakeTimePerSegment;
            yield return new WaitForSeconds(shakeTimePerSegment);
        }

        ownRigidBody.constraints = RigidbodyConstraints.None;
        transform.DOMove(placeToLand, durationForPieceBreak);
        transform.DOScale(smallPieceScaleTarget, durationForPieceBreak);
        yield return new WaitForSeconds(durationForPieceBreak);
        ownRigidBody.useGravity = true;

        SetSelfLayer(LayerDataObject.instance.puzzlePieceLayer);
    }


    public IEnumerator PlaceOnPuzzle(PieceTranformData destination, bool placedCorrectly, Vector3 placeToLandIfIncorrect)
    {
        SetSelfLayer(LayerDataObject.instance.ungrabablePuzzlePieceLayer);

        ownRigidBody.useGravity = false;
        ownRigidBody.constraints = RigidbodyConstraints.FreezePosition;

        transform.DOMove(destination.position, durationForPieceBreak);
        transform.DOScale(1, durationForPieceBreak);

        yield return new WaitForSeconds(durationForPieceBreak);

        if (placedCorrectly)
        {
            this.placedCorrectly = placedCorrectly;

            transform.DORotateQuaternion(destination.rotation, durationForPieceBreak);
            yield return new WaitForSeconds(durationForPieceBreak);

            transform.position = destination.position;
            transform.rotation = destination.rotation;

            ownRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            StartCoroutine(BreakPiece(placeToLandIfIncorrect));
        }
    }

    public void SetSelfLayer(int layer)
    {
        gameObject.layer = layer;
    }
}
