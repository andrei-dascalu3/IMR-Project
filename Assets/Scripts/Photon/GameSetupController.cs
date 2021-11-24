using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    public GameObject puzzleSphere;

    public PlayerSettingsData playerSettings;

    private void Awake()
    {
        GameObject playerSettingsGO = GameObject.Find("PlayerSettingsObject");

        playerSettings = playerSettingsGO.GetComponent<PlayerSettingsData>();

        puzzleSphere.GetComponent<PuzzleSetupManager>().puzzleTexture = playerSettings.puzzleTexture;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
