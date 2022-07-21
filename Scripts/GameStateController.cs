using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public static GameStateController instance;
    public string currentState { get; private set; } = "Live";
    public bool usingGamepad = false;
    
    void Start()
    {
        instance = this;
    }

    public void SetState(string newState)
    {
        if (newState == "Live")
            TimeManager.instance.timeFlowing = true;
        else
            TimeManager.instance.timeFlowing = false;

        currentState = newState;
        UiController.instance.ShowUi(newState);
    }
    
    public void ToggleInventory()
    {
        if (currentState == "Loot")
        {
            SetState("Live");
            LootController.instance.CloseContainer();
        }
        else if (currentState == "Inventory")
        {
            SetState("Live");
        }
        else
        {
            SetState("Inventory");
        }
    }
}
