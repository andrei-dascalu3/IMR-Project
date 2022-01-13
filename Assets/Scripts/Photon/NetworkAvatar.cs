using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(PhotonView))]
public class NetworkAvatar : MonoBehaviour
{
    public PhotonView pv;

    public Transform body;
    public Transform leftHand;
    public Transform rightHand;
   // public Transform head;
   // public Transform rightForeArm;
   // public Transform leftForeArm;
   // public Transform neck;

    public Transform xrBody;
    public Transform xrLeftHand;
    public Transform xrRightHand;
    // public Transform xrNeck;
    //public Transform xrRightForeArm;
    // public Transform xrLeftForeArm;
    //public Transform xrHead;

    public BrokenWorldPlayer ownXrAvatar;

    private void Start()
    {
        if (pv.IsMine)
        {
            SyncAvatarColor();
            FindOwnBodyParts();
            DisableOwnRenderers();
        }
    }

    private void SyncAvatarColor()
    {
        // ownXrAvatar = GameObject.Find(PhotonNetwork.NickName).GetComponent<BrokenWorldPlayer>();
        ownXrAvatar = BrokenWorldPlayer.player;
        Color color = ownXrAvatar.ownAvatarColor;
        pv.RPC(nameof(ChangeNetworkAvatarColor), RpcTarget.All, color.r, color.g, color.b);
    }

    private void DisableOwnRenderers()
    {
        //DisableOwnRendererForBodypart(neck);
        //DisableOwnRendererForBodypart(leftForeArm);
        //DisableOwnRendererForBodypart(rightForeArm);
        //DisableOwnRendererForBodypart(head);
        //DisableOwnRendererForBodypart(body);

        for (int i = 0; i < body.childCount; i++)
        {
            DisableOwnRendererForBodypart(body.GetChild(i));
        }

        DisableOwnRendererForBodypart(leftHand);
        DisableOwnRendererForBodypart(rightHand);
    }

    private void DisableOwnRendererForBodypart(Transform part)
    {
        for (int i = 0; i < part.childCount; i++)
        {
            part.GetChild(i).GetComponent<Renderer>().enabled = false;
        }
    }

    private void FindOwnBodyParts()
    {
        //GameObject ownXrRigObject = GameObject.Find(PhotonNetwork.NickName);

        //Transform ownCharacterTransform = ownXrRigObject.transform.Find("XR Rig - Character");

        //Transform cameraOffset = ownCharacterTransform.Find("Camera Offset");

        //xrBody = cameraOffset.Find("Main Camera - Body").Find("CharacterBodyNoArms");

        xrBody = ownXrAvatar.playerObject;
        //xrNeck = ownXrAvatar.neck;
        //xrLeftForeArm = ownXrAvatar.leftForeArm;
        //xrRightForeArm = ownXrAvatar.rightForeArm;
        //xrHead = ownXrAvatar.head;

        xrLeftHand = ownXrAvatar.leftArm;
        xrRightHand = ownXrAvatar.rightArm;

        //xrNeck = xrBody.Find("Neck");
        //xrLeftForeArm = xrBody.Find("Left-ForeArm");
        //xrRightForeArm = xrBody.Find("Right-ForeArm");
        //xrHead = xrBody.Find("Head");
        //xrBody = xrBody.Find("Body");

        //xrLeftHand = cameraOffset.Find("LeftHand Controller").Find("Left-Arm");
        //xrRightHand = cameraOffset.Find("RightHand Controller").Find("Right-Arm");
    }

    [PunRPC]
    private void BreakNetworkAvatar()
    {
        //ownXrAvatar.BreakCharacterPart(neck);
       // ownXrAvatar.BreakCharacterPart(leftForeArm);
       // ownXrAvatar.BreakCharacterPart(rightForeArm);
        //ownXrAvatar.BreakCharacterPart(head);
        //ownXrAvatar.BreakCharacterPart(body);

        for (int i = 0; i < body.childCount; i++)
        {
            ownXrAvatar.BreakCharacterPart(body.GetChild(i));
        }

        ownXrAvatar.BreakCharacterPart(leftHand);
        ownXrAvatar.BreakCharacterPart(rightHand);
    }

    [PunRPC]
    public void ChangeNetworkAvatarColor(float r, float g, float b)
    {
        AvatarColorer avatarColorer = new AvatarColorer();

        Color color = new Color(r, g, b);

        for (int i = 0; i < body.childCount; i++)
        {
            avatarColorer.ChangeAvatarPartColor(body.GetChild(i), color);
        }
        avatarColorer.ChangeAvatarPartColor(leftHand, color);
        avatarColorer.ChangeAvatarPartColor(rightHand, color);
    }

    void Update()
    {
        if (pv.IsMine)
        {
            body.position = xrBody.position;
            body.rotation = xrBody.rotation;

            leftHand.position = xrLeftHand.position;
            leftHand.rotation = xrLeftHand.rotation;

            rightHand.position = xrRightHand.position;
            rightHand.rotation = xrRightHand.rotation;

        }
    }

}
