using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItems : MonoBehaviour
{
    public InventoryItemSlot[] inventorySlots = new InventoryItemSlot[10];
    public Transform selectorT;

    ItemPlacer placer;
    HotbarItemSlot[] hotbarSlots = new HotbarItemSlot[10];

    int selectedSlot = 0;

    private void Start()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            hotbarSlots[i] = inventorySlots[i].hotbarSlot;
        }
        placer = GetComponentInChildren<ItemPlacer>();
    }

    public void UseActive()
    {
        InventoryItemData activeItem = inventorySlots[selectedSlot].itemData;

        if (!activeItem)
            return;
        else if (activeItem.useAction == "")
            return;

        switch (activeItem.useAction)
        {
            case "Place":
                if (placer.Use(activeItem.inWorldPrefab))
                    inventorySlots[selectedSlot].RemoveFromStack(1);

                if (!inventorySlots[selectedSlot].occupied)
                    placer.StopPlacement();
                break;
        }
    }

    public void NextItem()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            selectedSlot++;

            if (selectedSlot > 9)
                selectedSlot = 0;

            ChangeItem(selectedSlot);

            if (inventorySlots[selectedSlot].itemData)
                return;
        }
    }

    public void PreviousItem()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            selectedSlot--;

            if (selectedSlot < 0)
                selectedSlot = 9;

            ChangeItem(selectedSlot);
            
            if (inventorySlots[selectedSlot].itemData)
                return;
        }
    }

    public void ChangeItem(int selection)
    {
        selectedSlot = selection;
        selectorT.position = hotbarSlots[selectedSlot].transform.position;
        //Holding item animation
    }
}
