using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact(Transform player)
    {

    }

    public virtual string ActionText()
    {
        return "Interact";
    }
}
