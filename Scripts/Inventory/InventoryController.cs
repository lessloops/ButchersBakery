using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController instance;
    public GameObject inventoryUiObj;
    public List<InventoryItemSlot> inventorySlots;
    bool inventoryShowing;

    private void Awake()
    {
        instance = this;
        inventorySlots = new List<InventoryItemSlot>(inventoryUiObj.transform.GetComponentsInChildren<InventoryItemSlot>());
    }

    public bool CanPickup(WorldItem item)
    {
        if (inventorySlots.Find(x => x.itemData == item.itemData))
        {
            return true;
        }

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].itemData == null)
            {
                return true;
            }
        }
        return false;
    }

    public bool PickupItem(WorldItem item)
    {
        InventoryItemSlot targetSlot = inventorySlots.Find(x => x.itemData == item.itemData);

        if (targetSlot)
        {
            targetSlot.AddToStack(item.amount);
            return true;
        }

        for (int i = 0; i < inventorySlots.Count; i++)
        { 
            if (!inventorySlots[i].occupied)
            {
                inventorySlots[i].AddItem(item.itemData, item.amount);
                return true;
            }
        }

        return false;
    }

    public void Remove(InventoryItemData referenceData, int amount)
    {
        InventoryItemSlot targetSlot = inventorySlots.Find(x => x.itemData == referenceData);

        if (targetSlot)
            targetSlot.RemoveFromStack(amount);
    }
}
