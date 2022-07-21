using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{
    public override void Interact(Transform player)
    {
        TimeManager.instance.EndDay();
    }

    public override string ActionText()
    {
        return "Sleep";
    }
}
