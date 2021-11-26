using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwipeMapMenu : MonoBehaviour
{
    public Scrollbar scrollbar;
    float scroll_pos = 0;
    List<float> pos = new List<float>();

    void Update()
    {
        float distance = 1f / (pos.Count - 1f);

        for (int i = 0; i < pos.Count; i++)
        {
            pos[i] = distance * i;
        }
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.value;
        }
        else
        {
            for (int i = 0; i < pos.Count; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.value = Mathf.Lerp(scrollbar.value, pos[i], 0.1f);

                }
            }
        }
        for (int i = 0; i < pos.Count; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                for (int a = 0; a < pos.Count; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.7f, 0.7f), 0.1f);
                    }
                }
            }
        }
    }

    public Texture GetSelectedPhoto()
    {
        return null;
    }

}