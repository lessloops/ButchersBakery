using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    GameObject player;
    float inventoryAxisBuffer = 0.4f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        switch (GameStateController.instance.currentState)
        {
            case "Live":
                if (Input.GetButtonDown("Interact"))
                    player.GetComponent<Interact>().TryInteract();
                else if (Input.GetButtonDown("Inventory"))
                    GameStateController.instance.ToggleInventory();
                else if (Input.GetButtonDown("Item"))
                    player.GetComponent<UseItems>().UseActive();
                else if (Input.GetButtonDown("Cancel"))
                    player.GetComponentInChildren<ItemPlacer>().StopPlacement();
                else if (Input.GetButtonDown("Previous Item"))
                    player.GetComponent<UseItems>().PreviousItem();
                else if (Input.GetButtonDown("Next Item"))
                    player.GetComponent<UseItems>().NextItem();
                break;

            case "Inventory":
                CheckInventoryControls();
                break;

            case "Loot":
                CheckInventoryControls();
                break;

            case "Building":
                if (Input.GetButtonDown("Cancel"))
                    GameStateController.instance.SetState("Live");
                break;
                
            case "Placement":
                if (Input.GetButtonDown("Cancel"))
                    GameStateController.instance.SetState("Live");
                break;
        }
    }

    void CheckInventoryControls()
    {
        if (Input.GetButtonDown("Item"))
            InventoryCursor.instance.UseButtonPressed();
        else if (Input.GetButtonDown("Interact"))
            InventoryCursor.instance.UseButtonPressed();
        else if (Input.GetButtonDown("Inventory"))
            GameStateController.instance.ToggleInventory();

        if (GameStateController.instance.usingGamepad)
            CheckControllerInput();
        else
            CheckKeyboardMouseInput();
    }

    void CheckControllerInput()
    {
        if (Input.GetAxis("Vertical") > inventoryAxisBuffer)
        {
            InventoryCursor.instance.Move("Up");
        }
        else if (Input.GetAxis("Vertical") < -inventoryAxisBuffer)
        {
            InventoryCursor.instance.Move("Down");
        }
        else if (Input.GetAxis("Horizontal") < -inventoryAxisBuffer)
        {
            InventoryCursor.instance.Move("Left");
        }
        else if (Input.GetAxis("Horizontal") > inventoryAxisBuffer)
        {
            InventoryCursor.instance.Move("Right");
        }
        else
        {
            InventoryCursor.instance.shouldMove = true;
            InventoryCursor.instance.firstMove = true;
        }
    }

    void CheckKeyboardMouseInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            InventoryCursor.instance.Move("Up");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            InventoryCursor.instance.Move("Down");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            InventoryCursor.instance.Move("Left");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            InventoryCursor.instance.Move("Right");
        }
    }
}
