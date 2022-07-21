using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talkable : Interactable
{
    public Sprite portrait;
    public List<string> messages = new List<string>();
    int messageCounter = 0;
    bool speaking;
    DialogueController dc;

    private void Start()
    {
        dc = GameObject.FindGameObjectWithTag("GameController").GetComponent<DialogueController>();
    }

    public override void Interact(Transform player)
    {
        if (messageCounter < messages.Count)
        {
            dc.StartDialogue(portrait, messages[messageCounter]);
            messageCounter++;
        }
        else
            dc.StartDialogue(portrait, messages[messages.Count - 1]);
    }

    public override string ActionText()
    {
        return "Talk";
    }
}
