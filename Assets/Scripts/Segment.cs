using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum PortionPowerType
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Ability
}
public class Segment : MonoBehaviour
{
    [SerializeField] LayerMask portionLayer;
    public Transform originalParent;
    [SerializeField] public PortionPowerType powerType;
    [SerializeField] public int powerAmount = 1;
    [SerializeField] public SpriteRenderer valueSpriteRenderer;
    [SerializeField] public SpriteRenderer sliceSpriteRenderer;
    [SerializeField] Color[] colors;
    [SerializeField] Sprite[] valueSprites, sliceSprites;
    private void Start()
    {
        originalParent = transform.parent;

        if (powerAmount > 0) { valueSpriteRenderer.sprite = valueSprites[powerAmount - 1]; }
        sliceSpriteRenderer.sprite = GetCorrectSizeSliceSprite();
        AssignValueSpriteRendererSize();
        AssignSliceColour();

        CheckParent();

    }

    private void AssignValueSpriteRendererSize()
    {
        switch (FindObjectOfType<Wheel>().portions)
        {
            case 3:
                valueSpriteRenderer.transform.localScale = Vector3.one;
                break;
            case 5:
                valueSpriteRenderer.transform.localScale = Vector3.one * .75f;
                break;
            case 8:
                valueSpriteRenderer.transform.localScale = Vector3.one * .5f;
                break;
        }
    }

    private void AssignSliceColour()
    {
        switch (powerType)
        {
            case PortionPowerType.Addition:
                sliceSpriteRenderer.color = colors[0];
                break;
            case PortionPowerType.Subtraction:
                sliceSpriteRenderer.color = colors[1];
                break;
            case PortionPowerType.Multiplication:
                sliceSpriteRenderer.color = colors[2];
                break;
            case PortionPowerType.Division:
                sliceSpriteRenderer.color = colors[3];
                break;
            case PortionPowerType.Ability:
                sliceSpriteRenderer.color = colors[4];
                break;
        }
    }

    private Sprite GetCorrectSizeSliceSprite()
    {
        Sprite spriteToReturn = null; 
        switch (FindObjectOfType<Wheel>().portions)
        {
            case 3:
                spriteToReturn = sliceSprites[0];
                break;
            case 5:
                spriteToReturn = sliceSprites[1];
                break;
            case 8:
                spriteToReturn = sliceSprites[2];
                break;
        }
        return spriteToReturn;
    }

    public int ApplySegmentPower(int currentAmount)
    {
        int newAmount = currentAmount;
        switch (powerType)
        {
            case PortionPowerType.Addition:
                newAmount += powerAmount;
                break;
            case PortionPowerType.Subtraction:
                newAmount -= powerAmount;
                break;
            case PortionPowerType.Multiplication:
                newAmount *= powerAmount;
                break;
            case PortionPowerType.Division:
                newAmount /= powerAmount;
                break;
            case PortionPowerType.Ability:
                FindObjectOfType<Wheel>().ToggleDirection();
                break;
        }

        return newAmount;
    }

    private void OnMouseDown()
    {
        /*if(!isHeld)
        {
            bool playerAlreadyHoldingSegment = false;
            //check if any other segment is currently being carried by the player.
            foreach(Segment segment in FindObjectsOfType<Segment>())
            {
                if(segment != this && segment.isHeld)
                {
                    playerAlreadyHoldingSegment = true;
                }
            }
            if(!playerAlreadyHoldingSegment)
            {
                isHeld= true;
                transform.parent = null;
                if (portionHoveredOver != null)
                {
                    portionHoveredOver.isOccupied = false;
                    portionHoveredOver = null;
                }
            }
        }
        else
        {
            isHeld = false;
            if (portionHoveredOver == null || portionHoveredOver.isOccupied)
            {
                transform.parent = originalParent;
                transform.localPosition = Vector3.zero;
                transform.eulerAngles = originalRotation;
            }
            else
            {
                portionOccupying = portionHoveredOver;
                transform.parent = portionOccupying.transform;
                transform.localPosition = Vector3.zero;
                transform.eulerAngles = transform.parent.eulerAngles;
            }
        }*/
    }
    private void Update()
    {
        /*if(isHeld)
        {
            var mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
            transform.position = mousePos;

            RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 10000f, portionLayer);
            if(ray.collider !=null)
            {
                if(ray.collider.GetComponent<Portion>())
                {
                    portionHoveredOver = ray.collider.GetComponent<Portion>();
                    transform.eulerAngles = portionHoveredOver.transform.eulerAngles;
                }
            }
            else
            {
                portionHoveredOver = null;

            }

        }
        */
    }

    public void CheckParent()
    {
        originalParent.GetComponent<HotbarSlot>().CheckSegment();
        if (transform.parent == originalParent)
        {
            sliceSpriteRenderer.enabled = false;
            valueSpriteRenderer.enabled = false;
            if (GetComponent<PolygonCollider2D>() != null)
            {
                Destroy(sliceSpriteRenderer.gameObject.GetComponent<PolygonCollider2D>());
            }
            sliceSpriteRenderer.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            originalParent.GetComponent<SpriteRenderer>().color = Color.white;
            sliceSpriteRenderer.enabled = true;
            valueSpriteRenderer.enabled = true;

            if (GetComponent<PolygonCollider2D>() == null)
            {
                sliceSpriteRenderer.gameObject.AddComponent<PolygonCollider2D>();
            }
            sliceSpriteRenderer.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
