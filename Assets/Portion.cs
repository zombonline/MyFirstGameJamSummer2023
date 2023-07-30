using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Portion : MonoBehaviour
{
    [SerializeField] public bool isOccupied;
    private void Awake()
    {
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        isOccupied = transform.childCount > 0;
    }
}
