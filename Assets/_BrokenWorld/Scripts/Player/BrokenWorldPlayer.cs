using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class BrokenWorldPlayer : MonoBehaviour
{
    public static BrokenWorldPlayer player;

    public Transform playerObject;

    public Transform headCamera;

    public Transform head;
    public Transform body;
    public Transform leftArm;
    public Transform leftForeArm;
    public Transform rightArm;
    public Transform rightForeArm;
    public Transform neck;

    public XRRayInteractor leftController;
    public XRRayInteractor rightController;

    public Color ownAvatarColor;

    public Vector3 characterBreakCameraPositionTarget;
    public Quaternion characterBreakCameraRotationTarget;
    public float characterBreakCameraTransitionDuration;

    private void Awake()
    {
        player = this;

        ownAvatarColor = leftArm.GetChild(0).GetComponent<Renderer>().material.color; 

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += ReactivateCharacter;
    }

    public virtual void ReactivateCharacter(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(false);
            child.gameObject.SetActive(true);
        }
    }


    public void BreakCharacter()
    {
        RiseCamera();

        rightController.enabled = false;
        leftController.enabled = false;
        rightController.GetComponent<ActionBasedController>().enabled = false;
        leftController.GetComponent<ActionBasedController>().enabled = false;

        BreakCharacterPart(head);
        BreakCharacterPart(body);
        BreakCharacterPart(leftArm);
        BreakCharacterPart(leftForeArm);
        BreakCharacterPart(rightArm);
        BreakCharacterPart(rightForeArm);
        BreakCharacterPart(neck);
    }

    private void RiseCamera()
    {
        Transform fullBodyTransform = body.parent;
        Transform cameraTransform = fullBodyTransform.parent;
        fullBodyTransform.SetParent(cameraTransform.parent);

        cameraTransform.GetComponent<TrackedPoseDriver>().enabled = false;

        cameraTransform.DOLocalMove(characterBreakCameraPositionTarget, characterBreakCameraTransitionDuration);
        cameraTransform.DORotateQuaternion(characterBreakCameraRotationTarget, characterBreakCameraTransitionDuration);
    }

    public void BreakCharacterPart(Transform part)
    {
        for (int j = 0; j < part.childCount; j++)
        {
            part.GetChild(j).GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    internal void SetColor(Color color)
    {
        ownAvatarColor = color;
    }
}
