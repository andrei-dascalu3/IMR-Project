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

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            GameObject duplicate = Instantiate(buttonCopy);
            duplicate.transform.SetParent(contentButtons.transform, false);
            duplicate.transform.localPosition = new Vector3(0, 0, 0);
            RawImage rawImage = duplicate.GetComponentsInChildren<RawImage>()[0];
            rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
        
    }

}