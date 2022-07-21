using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    bool isShowing;
    SpriteRenderer sr;
    Transform playerT;
    PlacementCollider placementCol;
    GameObject placeableObject;

    void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        placementCol = GetComponentInChildren<PlacementCollider>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShowing)
            return;

        Vector2 playerFacing = playerT.GetComponent<TopDownCharacterController>().lastMotionVector;
        float x = (float)Math.Round((playerT.position.x + playerFacing.x) * 2) / 2;
        float y = (float)Math.Round((playerT.position.y + playerFacing.y) * 2) / 2;
        
        transform.position = new Vector2(x, y);

        CheckPlaceability();
    }
    
    public bool Use(GameObject prefab)
    {
        if (isShowing)
        {
            if (placementCol.canPlace)
            {
                PlaceItem(prefab);
                return true;
            }
        }
        else
        {
            StartPlacement(prefab);
        }

        return false;
    }

    void PlaceItem(GameObject itemPrefab)
    {
        GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);

        if (newItem.GetComponent<Plantable>())
            newItem.GetComponent<Plantable>().Plant();
    }

    void StartPlacement(GameObject itemPrefab)
    {
        isShowing = true;
        placeableObject = itemPrefab;
        sr.sprite = placeableObject.GetComponent<SpriteRenderer>().sprite;
        placementCol.SetCol(placeableObject.GetComponent<BoxCollider2D>().size, placeableObject.GetComponent<BoxCollider2D>().offset);
    }

    public void StopPlacement()
    {
        isShowing = false;
        placeableObject = null;
        sr.sprite = null;
    }

    private void CheckPlaceability()
    {
        if (placementCol.canPlace)
        {
            sr.color = new Color(0, 1, 0, 0.5f);
        }
        else
        {
            sr.color = new Color(1, 0, 0, 0.5f);
        }
    }
}
