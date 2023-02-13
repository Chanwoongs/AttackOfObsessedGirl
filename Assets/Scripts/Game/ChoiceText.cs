using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceText : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetSeleted(bool selected)
    {
        text.faceColor = selected ? Color.cyan : Color.black;
    }

    public TextMeshProUGUI TextField => text;
}
