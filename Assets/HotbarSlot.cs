using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HotbarSlot : MonoBehaviour
{
    [SerializeField] public SpriteRenderer numberSprite, tileSprite;
    [SerializeField] TextMeshPro numberText;
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] Sprite reverseDirectionSprite;

    Segment storedSegment;
    private void Start()
    {
        Invoke(nameof(CheckSegment), .01f);
    }

    public void CheckSegment()
    {
        storedSegment = transform.GetComponentInChildren<Segment>();

        if (storedSegment != null)
        {
            AssignFunctionAndNumberToText();
            AssignNumberSprite();
            CopySegmentAbilityColor();
        }
        else
        {
            numberSprite.sprite = null;
            tileSprite.color = Color.white;
            numberText.text = string.Empty;
        }
    }

    private void CopySegmentAbilityColor()
    {
        tileSprite.color = storedSegment.sliceSprite.color;
    }

    private void AssignNumberSprite()
    {
        if (storedSegment.powerAmount > 0)
        {
            numberSprite.sprite = numberSprites[storedSegment.powerAmount - 1];
        }
        else
        {
            switch (storedSegment.powerType)
            {
                case PortionPowerType.Ability:
                    numberSprite.sprite = reverseDirectionSprite;
                    break;
            }
        }
    }

    private void AssignFunctionAndNumberToText()
    {
        switch (storedSegment.powerType)
        {
            case PortionPowerType.Addition:
                numberText.text = "+";
                break;
            case PortionPowerType.Subtraction:
                numberText.text = "-";
                break;
            case PortionPowerType.Multiplication:
                numberText.text = "x";
                break;
            case PortionPowerType.Division:
                numberText.text = "÷";
                break;
            case PortionPowerType.Ability:
                break;
        }
        if (storedSegment.powerAmount > 0)
        {
            numberText.text = numberText.text + storedSegment.powerAmount.ToString();
        }
    }
}
