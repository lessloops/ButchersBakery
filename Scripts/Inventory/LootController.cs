using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootController : MonoBehaviour
{
    public static LootController instance;
    public GameObject lootUiObj;
    public Image lootUiContainerImg;
    public List<InventoryItemSlot> containerSlots { get; private set; }
    Lootable currentContainer;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        containerSlots = new List<InventoryItemSlot>(lootUiObj.transform.GetComponentsInChildren<InventoryItemSlot>());
    }

    public void Clear()
    {
        foreach (InventoryItemSlot slot in containerSlots)
        {
            slot.Clear();
        }
    }

    public void CloseContainer()
    {
        List<StoredItem> newContents = new List<StoredItem>();

        foreach (InventoryItemSlot slot in containerSlots)
        {
            if (slot.occupied)
            {
                StoredItem item = new StoredItem()
                {
                    data = slot.itemData,
                    stackSize = slot.stackSize,
                    columnIndex = slot.columnIndex,
                    rowIndex = slot.rowIndex
                };
                newContents.Add(item);
            }
        }

        InventoryCursor.instance.Position(false, 0, 0);
        currentContainer.contents = newContents;
    }

    public void Populate(Lootable container, Sprite containerSprite)
    {
        currentContainer = container;
        lootUiContainerImg.sprite = containerSprite;
        GameStateController.instance.SetState("Loot");
        Clear();

        foreach (StoredItem item in container.contents)
        {
            InventoryItemSlot targetSlot = containerSlots.Find(slot => slot.columnIndex == item.columnIndex && slot.rowIndex == item.rowIndex);
            targetSlot.AddItem(item.data, item.stackSize);
        }

        InventoryCursor.instance.Position(true, 0, 0);
    }
}
