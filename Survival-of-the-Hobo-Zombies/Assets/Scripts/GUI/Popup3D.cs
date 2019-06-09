using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup3D : MonoBehaviour
{

    public Text txtContent;    


    public void SetInfo(string text, Color color, int maxSize = 0)
    {
        txtContent.color = color;
        SetInfo(text, maxSize);
    }

    public void SetInfo(string text, int maxSize = 0)
    {
        txtContent.text = text;
        if (maxSize != 0)
            txtContent.resizeTextMaxSize = maxSize;
    }
}
