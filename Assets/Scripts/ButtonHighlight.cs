using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour
{

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        
    }

    public void MoveToHereY(int yVal)
    {
        rectTransform.anchoredPosition = new Vector2(0, yVal);
    }
}
