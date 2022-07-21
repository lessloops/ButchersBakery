using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InventoryItemSlot : MonoBehaviour
{
    public HotbarItemSlot hotbarSlot;
    public GameObject itemDisplay;
    public Text stackDisplay;
    public InventoryItemData itemData { get; private set; }
    public int columnIndex;
    public int rowIndex;
    public int stackSize { get; private set; }
    public bool occupied;


    public void AddItem(InventoryItemData reference, int amount)
    {
        itemData = reference;
        stackSize = amount;
        stackDisplay.text = stackSize.ToString();
        itemDisplay.GetComponent<Image>().sprite = itemData.icon;
        itemDisplay.SetActive(true);
        occupied = true;

        if (hotbarSlot)
            hotbarSlot.UpdateContents(this);
    }
    
    public void AddToStack(int amount)
    {
        stackSize += amount;
        stackDisplay.text = stackSize.ToString();

        if (hotbarSlot)
            hotbarSlot.UpdateContents(this);
    }

    public void GrabItem()
    {
        itemDisplay.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
    }

    public void RestoreColor()
    {
        itemDisplay.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
        stackDisplay.text = stackSize.ToString();

        if (stackSize == 0)
            Clear();
        else if (hotbarSlot)
            hotbarSlot.UpdateContents(this);
    }

    public void Clear()
    {
        itemDisplay.SetActive(false);
        stackSize = 0;
        stackDisplay.text = stackSize.ToString();
        itemData = null;
        itemDisplay.GetComponent<Image>().sprite = null;
        RestoreColor();
        occupied = false;
        
        if (hotbarSlot)
            hotbarSlot.ClearContents();
    }
}
