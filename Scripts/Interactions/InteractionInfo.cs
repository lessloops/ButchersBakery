using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionInfo : MonoBehaviour
{
    public GameObject highlighter;
    public RectTransform mainCanvasTransform;
    GameObject currentTarget;

    public void Highlight(GameObject target)
    {
        if (target == currentTarget)
            return;

        Vector3 pos = target.transform.position;
        
        if (GameStateController.instance.currentState == "Building" && target.GetComponent<Moveable>())
            highlighter.GetComponent<Text>().text = "Move";
        else if (target.GetComponent<Interactable>())
            highlighter.GetComponent<Text>().text = target.GetComponent<Interactable>().ActionText();
        
        Highlight(pos);
    }

    public void Highlight(Vector3 pos)
    {
        highlighter.SetActive(true);
        highlighter.GetComponent<RectTransform>().position = WorldToCanvas(mainCanvasTransform, pos);
    }

    public void Hide()
    {
        highlighter.SetActive(false);
        currentTarget = null;
    }

    Vector2 WorldToCanvas(RectTransform canv, Vector3 pos)
    {
        //Please note that this will only work if the element you're positioning is either a direct child of the main canvas or the child of a transform that stretches to the full extents of the canvas. Otherwise you have to take the offsets of all parent RectTransforms into account.
        if (Camera.main == null)
            return Vector2.zero;

        //This part is a bit complicated, but you have to make sure that you adjust your coordinates by the size of the canvas.
        Vector2 cPos = new Vector2(((Camera.main.WorldToViewportPoint(pos).x * canv.sizeDelta.x)), ((Camera.main.WorldToViewportPoint(pos).y * canv.sizeDelta.y)));
        
        return cPos;
    }
}
