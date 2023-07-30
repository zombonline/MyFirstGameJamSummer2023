using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentController : MonoBehaviour
{
    Segment segmentInHand;
    [SerializeField] LayerMask segmentLayer, portionLayer;
    [SerializeField] Transform cursor;
    public bool disabled = false;
    private void Update()
    {
        if(disabled)
        {
            return;
        }
        cursor.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (segmentInHand == null)
            {
                RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 10000f, segmentLayer);
                if (ray.collider != null && ray.collider.GetComponent<Segment>())
                {
                    segmentInHand = ray.collider.GetComponent<Segment>();
                    AssignSegmentInHandToTransform(cursor);
                }
            }

            else
            {
                RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 10000f, portionLayer);
                if (ray.collider != null && ray.collider.GetComponent<Portion>())
                {
                    var portionClicked = ray.collider.GetComponent<Portion>();

                    if (portionClicked.transform.childCount > 0)
                    {
                        var segmentToSwapOut = portionClicked.transform.GetChild(0).GetComponent<Segment>();
                        AssignSegmentInHandToTransform(portionClicked.transform);
                        segmentInHand = segmentToSwapOut;
                        AssignSegmentInHandToTransform(cursor);
                    }
                    else
                    {
                        AssignSegmentInHandToTransform(portionClicked.transform);
                        segmentInHand = null;
                    }
                }
                else
                {
                    AssignSegmentInHandToTransform(segmentInHand.originalParent);
                    segmentInHand = null;
                }
            }
        }

        if(segmentInHand != null)
        {
            RotateSegmentInHand();
        }
    }

    private void RotateSegmentInHand()
    {
        Portion portionHoveredOver;
        RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 10000f, portionLayer);
        if (ray.collider != null)
        {
            if (ray.collider.GetComponent<Portion>())
            {
                portionHoveredOver = ray.collider.GetComponent<Portion>();
                segmentInHand.transform.eulerAngles = portionHoveredOver.transform.eulerAngles;
            }
        }
        else
        {
            portionHoveredOver = null;
        }
    }

    private void AssignSegmentInHandToTransform(Transform transformParent)
    {
        segmentInHand.transform.parent = transformParent;
        segmentInHand.transform.localPosition = Vector3.zero;
        segmentInHand.transform.localEulerAngles = Vector3.zero;
        segmentInHand.CheckParent();
    }
}
