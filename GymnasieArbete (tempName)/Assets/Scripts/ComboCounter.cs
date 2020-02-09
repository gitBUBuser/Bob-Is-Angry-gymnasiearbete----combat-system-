using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    [SerializeField]
    Sprite[] numbers;
    [SerializeField]
    Sprite X;

    [SerializeField]
    SpriteRenderer tenRenderer;
    [SerializeField]
    SpriteRenderer onesRenderer;
    int currentComboIndex;

    public int ComboIndex { get { return currentComboIndex; } }

    public void Combo()
    {
        currentComboIndex++;
    }

    public void Reset()
    {
        currentComboIndex = 0;
    }

    public void Init(int currentComboIndex)
    {
        this.currentComboIndex = currentComboIndex;
    }
    
    void Update()
    {
        if (currentComboIndex >= 100)
        {
            tenRenderer.sprite = X;
            onesRenderer.sprite = X;
        }
        else
        {
            tenRenderer.sprite = numbers[(int)(currentComboIndex / 10)];
            onesRenderer.sprite = numbers[currentComboIndex % 10];
        }
    }
}
