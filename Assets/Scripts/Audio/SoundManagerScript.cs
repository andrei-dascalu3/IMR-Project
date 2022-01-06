using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip grabPiece, insertPiece, backgroundMusic;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        grabPiece = Resources.Load<AudioClip>("Audio/Fast Swish");
        insertPiece = Resources.Load<AudioClip>("Audio/Snap");
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "grab":
                audioSrc.time = 0.3f;
                audioSrc.PlayOneShot(grabPiece);
                break;
            case "insert":
                audioSrc.PlayOneShot(insertPiece);
                break;
            default:
                break;
        }
    }
}
