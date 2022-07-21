using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeCursor : MonoBehaviour
{
    public GameObject buildColPrefab;
    public PlacementCollider buildCol;
    public Vector2 cursorPos; //Used to determine if camera movement is required
    public InteractionInfo interactionInfo;
    public float speed;
    public Camera cam;
    GameObject currentTarget;
    Moveable liftedObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Moveable>())
        {
            currentTarget = collision.gameObject;
            interactionInfo.Highlight(currentTarget);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Moveable>())
        {
            currentTarget = null;
        }
    }

    public void Enable()
    {
        Vector2 point = cam.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, 0));
        transform.position = point;
        gameObject.SetActive(true);
        SetDisplay(true);
    }

    public void Disable()
    {
        if (liftedObject != null)
        {
            Destroy(buildCol.gameObject);
            liftedObject.Return();
            liftedObject = null;
        }

        SetDisplay(false);
        gameObject.SetActive(false);
    }

    private void SetDisplay(bool show)
    {
        GetComponent<SpriteRenderer>().enabled = show;
        GetComponent<Collider2D>().enabled = show;
    }

    private void Update()
    {
        if (GameStateController.instance.currentState != "Building")
            return;
        
        float x = transform.position.x + Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float y = transform.position.y + Input.GetAxis("Vertical") * speed * Time.deltaTime;

        float minX = cam.ScreenToWorldPoint(new Vector3(0,0, 0)).x;
        float maxX = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float minY = cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        float maxY = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;

        x = Mathf.Clamp(x, minX, maxX);
        y = Mathf.Clamp(y, minY, maxY);

        Vector2 newPos = new Vector2(x, y);
        transform.position = newPos;
        cam.GetComponent<CameraMovement>().CheckCamMovement(cam.WorldToScreenPoint(newPos));


        if (liftedObject != null)
        {
            CheckPlaceability();

            if (Input.GetButtonDown("Interact") && buildCol.canPlace)
                PlaceObject();
        }

        if (currentTarget == null)
            interactionInfo.Hide();
        else if (Input.GetButtonDown("Interact"))
            LiftObject(currentTarget.GetComponent<Moveable>());
    }

    private void LiftObject(Moveable target)
    {
        liftedObject = target;
        target.Lift(transform);
        buildCol = GameObject.Instantiate(buildColPrefab, target.transform).GetComponent<PlacementCollider>();
        buildCol.SetCol(target.GetComponent<BoxCollider2D>().size, target.GetComponent<BoxCollider2D>().offset);
        SetDisplay(false);
    }

    private void PlaceObject()
    {
        liftedObject.Drop();
        Destroy(buildCol.gameObject);
        liftedObject = null;
        SetDisplay(true);
    }

    private void CheckPlaceability()
    {
        if (buildCol.canPlace)
        {
            liftedObject.SetColors(new Color(0, 1, 0, 0.5f));
        }
        else
        {
            liftedObject.SetColors(new Color(1, 0, 0, 0.5f));
        }
    }
}
