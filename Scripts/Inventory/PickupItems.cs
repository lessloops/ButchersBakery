using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItems : MonoBehaviour
{
    public CollisionDetection pickupDetection;

    // Start is called before the first frame update
    void Start()
    {
        pickupDetection = transform.Find("PickupDetector").GetComponent<CollisionDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForItems();
    }

    void CheckForItems()
    {
        foreach (Collider2D col in pickupDetection.collisions)
        {
            if (col.GetComponent<WorldItem>())
            {
                WorldItem foundItem = col.GetComponent<WorldItem>();

                if (InventoryController.instance.CanPickup(foundItem))
                {
                    foundItem.MoveTowards(transform);
                }
            }
        }
    }
}
