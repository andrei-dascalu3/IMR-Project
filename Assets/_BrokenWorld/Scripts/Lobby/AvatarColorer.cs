using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarColorer
{
    public void ChangeAvatarColor(BrokenWorldPlayer avatar, Color color)
    {
        ChangeAvatarPartColor(avatar.head, color);
        ChangeAvatarPartColor(avatar.leftArm, color);
        ChangeAvatarPartColor(avatar.leftForeArm, color);
        ChangeAvatarPartColor(avatar.rightArm, color);
        ChangeAvatarPartColor(avatar.rightForeArm, color);
        ChangeAvatarPartColor(avatar.neck, color);
        ChangeAvatarPartColor(avatar.body, color);
    }

    public void ChangeAvatarPartColor(Transform part, Color color)
    {
        for (int i = 0; i < part.childCount; i++)
        {
            part.GetChild(i).GetComponent<Renderer>().material.color = color;
        }
    }
}
