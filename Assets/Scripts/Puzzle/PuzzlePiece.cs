using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzlePiece : MonoBehaviour
{
    public Rigidbody ownRigidBody;

    public const float durationForPieceBreak = 3; //2
    public const float smallPieceScaleTarget = 0.1f;

    public const float shakeSegments = 10;
    public const float shakeTimePerSegment = 2 / shakeSegments;
    public const float shakeStrenthMultiplier = 3;

    public bool placedCorrectly = false;

    private void Awake()
    {
        ownRigidBody = GetComponent<Rigidbody>();
    }

    public IEnumerator BreakPiece(Vector3 placeToLand)
    {
        ownRigidBody.constraints = RigidbodyConstraints.FreezePosition;

        float timer = 0f;
        while (timer < shakeTimePerSegment * shakeSegments)
        {
            transform.DOShakeRotation(shakeTimePerSegment, 3 + shakeStrenthMultiplier * timer * (5 / (shakeTimePerSegment * shakeSegments)), 5, 3, false);
            timer += shakeTimePerSegment;
            yield return new WaitForSeconds(shakeTimePerSegment);
        }

        ownRigidBody.constraints = RigidbodyConstraints.None;
        transform.DOMove(placeToLand, durationForPieceBreak);
        transform.DOScale(smallPieceScaleTarget, durationForPieceBreak);
        yield return new WaitForSeconds(durationForPieceBreak);
        ownRigidBody.useGravity = true;
    }


    public IEnumerator PlaceOnPuzzle(PieceTranformData destination, bool placedCorrectly, Vector3 placeToLandIfIncorrect)
    {
        ownRigidBody.useGravity = false;
        ownRigidBody.constraints = RigidbodyConstraints.FreezePosition;
        transform.DOMove(destination.position, durationForPieceBreak);
        Debug.Log(destination.position);
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
}
