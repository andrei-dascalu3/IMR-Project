using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UploadImageScript : MonoBehaviour
{
    string path;
    public GameObject newImageButton;
    public RawImage uploadedImage;
    
    public void OpenExplorer()
    {
        path = EditorUtility.OpenFilePanel("Overwrite with jpg", "", "jpg");
        GetImage();
    }
    void GetImage()
    {
        if (path != null)
        {
            UpdateImage();
        }
    }
    void UpdateImage()
    {
        WWW www = new WWW("file:///" + path);
        newImageButton.SetActive(true);
        uploadedImage.texture = www.texture;
        
    }
}
