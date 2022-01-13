using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarColorManager : MonoBehaviour
{
    private BrokenWorldPlayer avatar;
    public List<Button> colorButtons;

    public Button nextButton;

    public GameObject helpMenu;
    public GameObject settingsMenu;
    public GameObject colorMenu;

    public NetworkAvatar networkAvatar;

    private AvatarColorer colorer = new AvatarColorer();

    void Start()
    {
        GetAvatarsFromScene();

        SubscribeButtonsFunction();
    }

    private void GetAvatarsFromScene()
    {
        avatar = BrokenWorldPlayer.player;

        networkAvatar = PhotonRoomController.room.ownNetworkAvatar;
    }

    private void SubscribeButtonsFunction()
    {
        for(int i = 0; i < colorButtons.Count; i++)
        {
            Color color = colorButtons[i].transform.GetChild(0).GetComponent<Image>().color;
            colorButtons[i].onClick.AddListener(delegate { colorer.ChangeAvatarColor(avatar, color); });
            colorButtons[i].onClick.AddListener(delegate { avatar.SetColor(color); });
            colorButtons[i].onClick.AddListener(delegate { ChangeNetworkAvatarColor(color); });
        }

        if (PhotonNetwork.IsMasterClient)
        {
            nextButton.onClick.AddListener(delegate { settingsMenu.SetActive(true); });
            nextButton.onClick.AddListener(delegate { colorMenu.SetActive(false); });
        }
        else
        {
            nextButton.onClick.AddListener(delegate { helpMenu.SetActive(true); });
            nextButton.onClick.AddListener(delegate { colorMenu.SetActive(false); });
        }
    }

    private void ChangeNetworkAvatarColor(Color color)
    {
        networkAvatar.pv.RPC(nameof(ChangeNetworkAvatarColor), RpcTarget.AllBuffered, color.r, color.g, color.b);
    }
}
