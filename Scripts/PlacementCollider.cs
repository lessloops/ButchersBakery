using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementCollider : MonoBehaviour
{
    public bool canPlace;
    public List<Collider2D> collidingObjects = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canPlace = false;
        collidingObjects.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collidingObjects.Remove(collision);
    }

    public void SetCol(Vector2 size, Vector2 offset)
    {
        GetComponent<BoxCollider2D>().size = size;
        GetComponent<BoxCollider2D>().offset = offset;
    }

    private void Update()
    {
        if (collidingObjects.Count == 0)
            canPlace = true;
    }
}
