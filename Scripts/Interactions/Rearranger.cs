using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rearranger : Interactable
{
    public override void Interact(Transform player)
    {
        GameStateController.instance.SetState("Building");
    }

    public override string ActionText()
    {
        return "Rearrange";
    }
}
