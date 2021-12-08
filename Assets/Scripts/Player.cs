using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public Transform playerObject;

    public Transform[] bodyParts;

    // Start is called before the first frame update
    void Start()
    {
        //Invoke("BreakCharacter", 5);
    }

    public void BreakCharacter()
    {
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    for (int j = 0; j < transform.GetChild(i).childCount; j++)
        //    {
        //        transform.GetChild(i).GetChild(j).GetComponent<Rigidbody>().isKinematic = false;
        //    }
        //}

        for (int i = 0; i < bodyParts.Length; i++)
        {
            for (int j = 0; j < bodyParts[i].childCount; j++)
            {
                bodyParts[i].GetChild(j).GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
