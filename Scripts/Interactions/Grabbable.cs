using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : Interactable
{
    public float grabbedPosOffset;

    public override void Interact(Transform player)
    {
        player.GetComponent<Interact>().HoldItem(this);
        transform.SetParent(player);
        GetComponent<Collider2D>().enabled = false;
    }

    public void Drop()
    {
        transform.parent = null;
        GetComponent<Collider2D>().enabled = true;
    }

    public override string ActionText()
    {
        return "Grab";
    }
}
