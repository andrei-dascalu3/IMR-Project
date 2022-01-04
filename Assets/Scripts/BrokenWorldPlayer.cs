using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BrokenWorldPlayer : MonoBehaviour
{
    public Transform playerObject;

    public Transform headCamera;

    public Transform head;
    public Transform body;
    public Transform leftArm;
    public Transform leftForeArm;
    public Transform rightArm;
    public Transform rightForeArm;
    public Transform neck;

    private Quaternion headStartRotation;

    private void Awake()
    {
        headStartRotation = head.rotation;
    }

    public void BreakCharacter()
    {
        BreakCharacterPart(head);
        BreakCharacterPart(body);
        BreakCharacterPart(leftArm);
        BreakCharacterPart(leftForeArm);
        BreakCharacterPart(rightArm);
        BreakCharacterPart(rightForeArm);
        BreakCharacterPart(neck);
    }

    public void BreakCharacterPart(Transform part)
    {
        for (int j = 0; j < part.childCount; j++)
        {
            part.GetChild(j).GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    //public void Update()
    //{
        //head.rotation = headCamera.rotation;

        //head.rotation = new Quaternion(
        //    headCamera.rotation.x + headStartRotation.x,
        //    headCamera.rotation.y + headStartRotation.y,
        //    headCamera.rotation.z + headStartRotation.y,
        //    headCamera.rotation.w);

        //head.localRotation = new Quaternion(head.localRotation.x, headCamera.rotation.x, headCamera.rotation.y, head.localRotation.w);

    //    head.rotation = new Quaternion(
    //headCamera.rotation.x + headStartRotation.x,
    //headCamera.rotation.y + headStartRotation.y,
    //headCamera.rotation.z + headStartRotation.z,
    //headCamera.rotation.w);

        //    body.rotation = new Quaternion(
        //        body.rotation.x, 
        //        head.rotation.y, 
        //        body.rotation.z, 
        //        body.rotation.w);

        //    leftForeArm.rotation = new Quaternion(
        //        leftForeArm.rotation.x, 
        //        head.rotation.y, 
        //        leftForeArm.rotation.z, 
        //        leftForeArm.rotation.w);

        //    rightForeArm.rotation = new Quaternion(
        //        rightForeArm.rotation.x, 
        //        head.rotation.y, 
        //        rightForeArm.rotation.z, 
        //        rightForeArm.rotation.w);
    //}
    }
