using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UploadImageScript : MonoBehaviour
{
    string path;
    // public GameObject newImageButton;
    public GameObject contentButtons;
    public GameObject buttonCopy;

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

        /*GameObject button = new GameObject();
        button.transform.parent = contentButtons.transform;
        button.AddComponent<RectTransform>();
        button.AddComponent<Button>();

        GameObject[] childrenButtons = contentButtons.GetComponentsInChildren<GameObject>();
        int count = childrenButtons.Length - 1;
        button.transform.position = childrenButtons[count].transform.position;
        button.transform.localScale = childrenButtons[0].GetComponent<Renderer>().bounds.size;

        button.AddComponent<RawImage>();
        RawImage uploadedImage = button.GetComponent<RawImage>();
        uploadedImage.texture = www.texture;*/
        
        GameObject duplicate = Instantiate(buttonCopy); ;
        duplicate.transform.parent = contentButtons.transform;
        duplicate.transform.localPosition = new Vector3(0, 0, 0);
        RawImage rawImage = duplicate.GetComponentsInChildren<RawImage>()[0];
        rawImage.texture = www.texture;
        GameObject[] kids = contentButtons.GetComponentsInChildren<GameObject>();
        GameObject temp = kids[kids.Length - 1];
        kids[kids.Length - 1] = kids[kids.Length - 2];
        kids[kids.Length - 2] = temp;
    }
}