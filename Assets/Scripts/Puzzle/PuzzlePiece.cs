using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzlePiece : MonoBehaviour
{
    private Transform placeToLand;

    public const float durationForPieceBreak = 3; //2
    public const float smallPieceScaleTarget = 0.1f;

    //public const float shakeSpeed = 1; //how fast it shakes
    //public const float shakeAmount = 0.2f; //how much it shakes
    public const float shakeSegments = 10;
    public const float shakeTimePerSegment = 2 / shakeSegments;
    public const float shakeStrenthMultiplier = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    public IEnumerator BreakPiece()
    {
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

        float timer = 0f;
        while (timer < shakeTimePerSegment * shakeSegments)
        {
            transform.DOShakeRotation(shakeTimePerSegment, 3 + shakeStrenthMultiplier * timer * (5 / (shakeTimePerSegment * shakeSegments)), 5, 3, false);
            timer += shakeTimePerSegment;
            yield return new WaitForSeconds(shakeTimePerSegment);
        }
        
        ////yield return new WaitForSeconds(shakeTime / 2);
        //transform.DOShakeRotation(shakeTime / 2, 15, 10, 10, false).;
        //yield return new WaitForSeconds(shakeTime);
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        transform.DOMove(placeToLand.position, durationForPieceBreak);
        transform.DOScale(smallPieceScaleTarget, durationForPieceBreak);
        yield return new WaitForSeconds(durationForPieceBreak);
        transform.GetComponent<Rigidbody>().useGravity = true;
    }

    public void SetPlaceToLand(Transform place)
    {
        placeToLand = place;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
