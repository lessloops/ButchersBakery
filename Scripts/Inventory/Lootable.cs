using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StoredItem
{
    public InventoryItemData data;
    public int stackSize;
    public int columnIndex;
    public int rowIndex;
}

public class Lootable : Interactable
{
    public Sprite lootViewAvatar;
    public List<StoredItem> contents = new List<StoredItem>();
    
    public override void Interact(Transform player)
    {
        LootController.instance.Populate(this, lootViewAvatar);
    }

    public override string ActionText()
    {
        return "Open";
    }
}
