using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(PhotonView))]
public class NetworkAvatar : MonoBehaviour
{
    public PhotonView pv;

    public Transform head;
    public Transform torso;
    public Transform leftHand;
    public Transform rightHand;

    public Transform xrHead;
    public Transform xrTorso;
    public Transform xrLeftHand;
    public Transform xrRightHand;

    private void Start()
    {
        if (pv.IsMine)
        {
            GameObject ownXrRigObject = GameObject.Find(PhotonNetwork.NickName);

            Transform ownCharacterTransform = ownXrRigObject.transform.GetChild(1);

            Transform cameraOffset = ownCharacterTransform.GetChild(0);

            xrHead = cameraOffset.GetChild(0).GetChild(0);
            xrLeftHand = cameraOffset.GetChild(1).GetChild(0);
            xrRightHand = cameraOffset.GetChild(2).GetChild(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            head.position = xrHead.position;
            head.rotation = xrHead.rotation;

            leftHand.position = xrLeftHand.position;
            leftHand.rotation = xrLeftHand.rotation;

            rightHand.position = xrRightHand.position;
            rightHand.rotation = xrRightHand.rotation;

        }
    }

}
