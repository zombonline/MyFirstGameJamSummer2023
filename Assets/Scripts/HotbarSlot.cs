using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HotbarSlot : MonoBehaviour
{

    //This script will be attached to each slot on the hotbar at the bottom of the game screen.
    //It will manage the info displayed on the slot and when it should be shown.
    [SerializeField] public SpriteRenderer numberSprite, tileSprite;
    [SerializeField] TextMeshPro numberText;
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] Sprite reverseDirectionSprite;

    Segment storedSegment;

    public void CheckSegment()
    {
        //check if a segment is stored inside the slot.
        storedSegment = transform.GetComponentInChildren<Segment>();

        //if a segment is stored, use the info from that segment and assign the slots information.
        if (storedSegment != null)
        {
            AssignFunctionAndNumberToText();
            AssignNumberSprite();
            CopySegmentAbilityColor();
        }
        //if a segment is NOT stored, remove all information and display a blank slot.
        else
        {
            numberSprite.sprite = null;
            tileSprite.color = Color.white;
            numberText.text = string.Empty;
        }
    }

    private void CopySegmentAbilityColor()
    {
        //change slot background color to match the segment color.
        tileSprite.color = storedSegment.sliceSprite.color;
    }

    private void AssignNumberSprite()
    {
        //Display a sprite that shows the value of the segment (1-9)
        if (storedSegment.powerAmount > 0)
        {
            numberSprite.sprite = numberSprites[storedSegment.powerAmount - 1];
        }
        //If the segment is a special power and has no value, then selected the specific sprite to display
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
        //change text in corner of the slot to show what the segment stored inside does e.g. "+", "-".
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
                numberText.text = "�";
                break;
            case PortionPowerType.Ability:
                break;
        }
        //if the stored segment's number is greater than 0, add its value to the string.
        if (storedSegment.powerAmount > 0)
        {
            numberText.text = numberText.text + storedSegment.powerAmount.ToString();
        }
    }
}
