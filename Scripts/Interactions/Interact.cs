using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public InteractionInfo interactionInfo;
    CollisionDetection interactionDetection;
    Grabbable heldItem;
    Interactable currentTarget;
    TopDownCharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<TopDownCharacterController>();
        interactionDetection = transform.Find("InteractionDetector").GetComponent<CollisionDetection>();
    }

    private void Update()
    {
        if (GameStateController.instance.currentState == "Live")
        {
            if (heldItem)
                PositionHeldItem();
            else
                CheckInteractables();
        }
    }

    public void TryInteract()
    {
        if (heldItem)
        {
            DropItem();
        }
        else
        {
            if (currentTarget)
            {
                currentTarget.Interact(transform);
                GetComponentInChildren<ItemPlacer>().StopPlacement();
            }
        }
    }

    void CheckInteractables()
    {
        GameObject closest = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider2D col in interactionDetection.collisions)
        {
            if (col.GetComponent<Interactable>())
            {
                Vector3 directionToTarget = col.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closest = col.gameObject;
                }
            }
        }

        if (closest != null)
        {
            interactionInfo.Highlight(closest);
            currentTarget = closest.GetComponent<Interactable>();
        }
        else
        {
            interactionInfo.Hide();
            currentTarget = null;
        }
    }

    public void HoldItem(Grabbable grabbedItem)
    {
        heldItem = grabbedItem;
        interactionInfo.Hide();
        characterController.flipAnimations = true;
        transform.position = grabbedItem.transform.position;
        characterController.speed *= 0.5f;
    }

    void DropItem()
    {
        heldItem.Drop();
        heldItem = null;
        characterController.flipAnimations = false;
        characterController.speed *= 2;
    }

    void PositionHeldItem()
    {
        heldItem.transform.localPosition = characterController.lastMotionVector * -heldItem.grabbedPosOffset;
    }
}
