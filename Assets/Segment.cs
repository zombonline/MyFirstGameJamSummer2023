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
    [SerializeField] public SpriteRenderer abilitySprite, sliceSprite;
    [SerializeField] Sprite sliceThree, sliceFive, sliceEight;
    [SerializeField] Color[] colors;
    [SerializeField] Sprite[] valueSprites;
    
    private void Start()
    {
        originalParent = transform.parent;

        sliceSprite.sprite = sliceThree;
        switch (powerType)
        {
            case PortionPowerType.Addition:
                sliceSprite.color = colors[0]; 
                abilitySprite.sprite = valueSprites[powerAmount - 1];
                break;
            case PortionPowerType.Subtraction:
                sliceSprite.color = colors[1];
                abilitySprite.sprite = valueSprites[powerAmount - 1];
                break;
            case PortionPowerType.Multiplication:
                sliceSprite.color = colors[2];
                abilitySprite.sprite = valueSprites[powerAmount - 1];
                break;
            case PortionPowerType.Division:
                sliceSprite.color = colors[3];
                abilitySprite.sprite = valueSprites[powerAmount - 1];
                break;
            case PortionPowerType.Ability:
                sliceSprite.color = colors[4];
                break;
        }
        CheckParent();

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
                FindObjectOfType<Wheel>().spinDirection = -FindObjectOfType<Wheel>().spinDirection;
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
            sliceSprite.enabled = false;
            abilitySprite.enabled = false;
            if (GetComponent<PolygonCollider2D>() != null)
            {
                Destroy(sliceSprite.gameObject.GetComponent<PolygonCollider2D>());
            }
            sliceSprite.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            originalParent.GetComponent<SpriteRenderer>().color = Color.white;
            sliceSprite.enabled = true;
            if (GetComponent<PolygonCollider2D>() == null)
            {
                sliceSprite.gameObject.AddComponent<PolygonCollider2D>();
            }
            sliceSprite.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
