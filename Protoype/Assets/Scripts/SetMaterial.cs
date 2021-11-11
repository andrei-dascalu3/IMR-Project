using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterial : MonoBehaviour
{
    public Texture mainTexture;
    void Start()
    {
        Renderer[] pieces = this.transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer piece in pieces)
        {
            Debug.Log(piece.name);
            piece.material.EnableKeyword("_NORMALMAP");
            piece.material.SetTexture("_MainTex", mainTexture);
        }
    }
}