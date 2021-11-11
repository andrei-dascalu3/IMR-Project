using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterial : MonoBehaviour
{
    public Texture mainTexture;
    private Renderer renderer;
    void Start()
    {
        Transform[] jigsawPieces = this.transform.GetComponentsInChildren<Transform>();
        Debug.Log(jigsawPieces.Length);
        for (int i = 0; i < jigsawPieces.Length; i++)
        {
            Debug.Log(jigsawPiece.name);
            renderer = jigsawPiece.gameObject.GetComponent<Renderer>();
            renderer.material.EnableKeyword("_NORMALMAP");
            renderer.material.EnableKeyword("_METALLICGLOSSMAP");
            renderer.material.SetTexture("_MainTex", mainTexture);
            renderer.material.SetTexture("_BumpMap", mainTexture);
        }
    }
}
}
