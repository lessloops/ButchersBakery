using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public InventoryItemData itemData;
    public int amount;
    public float moveSpeed = 10.0f;
    public float bounceSpeed = 1f;
    public float bounceHeight = 0.05f;
    public float bounceDistance = 1f;

    bool moving;
    bool bouncing;
    bool falling;
    float currentHeight;
    Transform moveTarget;
    Vector2 bounceTarget;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
    }

    public WorldItem(InventoryItemData data)
    {
        itemData = data;
    }

    public void Spawn(Vector3 spawnPoint)
    {
        bouncing = true;
        float randX = Random.Range(bounceDistance/3, bounceDistance);

        if (Random.Range(0f, 1f) > 0.5f)
            randX = -randX;

        float landingX = spawnPoint.x + randX;
        bounceTarget = new Vector2(landingX, spawnPoint.y);
    }

    public void MoveTowards(Transform target)
    {
        if (!moving)
        {
            moving = true;
            moveTarget = target;
        }
    }

    private void Update()
    {
        if (bouncing)
        {
            moving = false;
            float step = bounceSpeed * Time.deltaTime;

            if (falling)
            {
                currentHeight -= step;
            }
            else
            {
                currentHeight += step;
            }

            if (currentHeight >= bounceHeight)
                falling = true;

            float newY = bounceTarget.y + currentHeight;
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(bounceTarget.x, newY), step);

            if (currentHeight <= 0)
            {
                bouncing = false;
            }
        }
        else if (moving)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, moveTarget.position, step);

            if (Vector3.Distance(transform.position, moveTarget.position) < 0.01f)
            {
                InventoryController.instance.PickupItem(this);
                Destroy(gameObject);
            }
        }
    }
}
