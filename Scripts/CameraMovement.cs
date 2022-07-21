using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed = 1.0f;
    public float buildingMoveSpeed;

    List<string> trackingGameStates = new List<string>() { "Live", "Dialogue" };
    private float cursorPadding = 10;
    private Vector3 offset;
    private Vector3 targetPos;

    private void Start()
    {
        if (target == null) return;

        offset = transform.position - target.position;
    }

    private void Update()
    {
        if (trackingGameStates.Contains(GameStateController.instance.currentState))
        {
            if (target == null) return;

            targetPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }
    }

    public void CheckCamMovement(Vector2 cursorPos)
    {
        float x = transform.position.x;
        float y = transform.position.y;

        if (cursorPos.x <= cursorPadding)
            x -= buildingMoveSpeed * Time.deltaTime;
        else if (cursorPos.x >= Screen.width - cursorPadding)
            x += buildingMoveSpeed * Time.deltaTime;

        if (cursorPos.y <= cursorPadding)
            y -= buildingMoveSpeed * Time.deltaTime;
        else if (cursorPos.y >= Screen.height - cursorPadding)
            y += buildingMoveSpeed * Time.deltaTime;

        transform.position = new Vector2(x, y);
    }
}
