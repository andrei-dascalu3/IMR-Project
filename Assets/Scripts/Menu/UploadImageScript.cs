using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Networking;

public class UploadImageScript : MonoBehaviour
{
    string path;
    public GameObject contentButtons;
    public GameObject buttonCopy;

    public Texture newTexture;

    public void OpenExplorer()
    {
        path = EditorUtility.OpenFilePanel("Show all images", "", "jpg,png,jpeg");
        StartCoroutine(GetTexture());
        
    }
    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            GameObject duplicate = Instantiate(buttonCopy); ;
            duplicate.transform.parent = contentButtons.transform;
            duplicate.transform.localPosition = new Vector3(0, 0, 0);
            RawImage rawImage = duplicate.GetComponentsInChildren<RawImage>()[0];
            rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture; 
            GameObject[] kids = contentButtons.GetComponentsInChildren<GameObject>();
            GameObject temp = kids[kids.Length - 1];
            kids[kids.Length - 1] = kids[kids.Length - 2];
            kids[kids.Length - 2] = temp;
        }
        
    }

}