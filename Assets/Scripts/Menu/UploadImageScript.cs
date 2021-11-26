using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Networking;

public class UploadImageScript : MonoBehaviour
{
    string path;
    // public GameObject newImageButton;
    public GameObject contentButtons;
    public GameObject buttonCopy;

    public Texture newTexture;

    public void OpenExplorer()
    {
        //path = EditorUtility.OpenFilePanel("Overwrite with jpg", "", "jpg");
        GetImage();
    }
    void GetImage()
    {
        if (path != null)
        {
            StartCoroutine(UpdateImage()); //UpdateImage();
        }
    }
    IEnumerator UpdateImage() //void UpdateImage()
    {
        StartCoroutine(GetRequest("file:///" + path));
        yield return new WaitForSeconds(0.1f);
        //WWW www = new WWW("file:///" + path);

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
        rawImage.texture = newTexture; // www.texture;
        GameObject[] kids = contentButtons.GetComponentsInChildren<GameObject>();
        GameObject temp = kids[kids.Length - 1];
        kids[kids.Length - 1] = kids[kids.Length - 2];
        kids[kids.Length - 2] = temp;
    }
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    newTexture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

}