using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip grabPiece, insertPiece, backgroundMusic, clickedButtonSound, clickedOptionSound, explosionSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        grabPiece = Resources.Load<AudioClip>("Audio/Fast Swish");
        insertPiece = Resources.Load<AudioClip>("Audio/Snap");
        clickedButtonSound = Resources.Load<AudioClip>("Audio/ButtonClicked");
        clickedOptionSound = Resources.Load<AudioClip>("Audio/OptionClicked");
        explosionSound = Resources.Load<AudioClip>("Audio/Shockwave");
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
            case "explosion":
                audioSrc.PlayOneShot(explosionSound);
                break;
            default:
                break;
        }
    }

    public static void ClickedButtonSound()
    {
        audioSrc.PlayOneShot(clickedButtonSound);
    }

    public static void ClickedOptionSound()
    {
        audioSrc.PlayOneShot(clickedOptionSound);
    }
}
