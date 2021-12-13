using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BrokenWorldPlayer : MonoBehaviour
{
    public Transform playerObject;

    public Transform[] bodyParts;

    public void BreakCharacter()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            for (int j = 0; j < bodyParts[i].childCount; j++)
            {
                bodyParts[i].GetChild(j).GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
