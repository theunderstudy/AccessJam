using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    public Text MainText;

    public void Init(string DisplayText , Color color)
    {
        MainText.text = DisplayText;
        MainText.color = color;
    }
}
