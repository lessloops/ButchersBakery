using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCursor : MonoBehaviour
{
    public static InventoryCursor instance;
    public GameObject InventoryUi;
    public GameObject LootUi;
    public bool shouldMove = true;
    public bool firstMove = true;
    [SerializeField] Vector2 cursorOffset = new Vector2();

    List<InventoryItemSlot> inventorySlots;
    List<InventoryItemSlot> lootSlots;
    InventoryItemSlot grabSlot;
    InventoryItemSlot selectedSlot;
    InventoryItemSlot grabbedItemOriginalSlot;
    int columnIndex;
    int rowIndex;
    bool inLootContainer;

    private void Awake()
    {
        instance = this;
        inventorySlots = new List<InventoryItemSlot>(InventoryUi.GetComponentsInChildren<InventoryItemSlot>());
        lootSlots = new List<InventoryItemSlot>(LootUi.GetComponentsInChildren<InventoryItemSlot>());
    }
    
    void Start()
    {
        grabSlot = GetComponentInChildren<InventoryItemSlot>();
        selectedSlot = inventorySlots.Find(x => x.columnIndex == columnIndex && x.rowIndex == rowIndex);
        Position(false, 0, 0);
        gameObject.SetActive(false);
    }

    public void Move(string direction)
    {
        if (!shouldMove)
            return;

        switch (direction)
        {
            case "Up":
                if (rowIndex > 0)
                {
                    rowIndex--;
                }
                else if (!inLootContainer && GameStateController.instance.currentState == "Loot")
                {
                    inLootContainer = true;
                    rowIndex = 3;
                }
                break;

            case "Down":
                if (rowIndex < 3)
                {
                    rowIndex++;
                }
                else if (inLootContainer)
                {
                    inLootContainer = false;
                    rowIndex = 0;
                }
                break;

            case "Left":
                if (columnIndex > 0)
                    columnIndex--;
                break;

            case "Right":
                if (columnIndex < 9)
                    columnIndex++;
                break;
        }

        Position(inLootContainer, columnIndex, rowIndex);

        if (GameStateController.instance.usingGamepad)
        {
            shouldMove = false;

            if (firstMove)
            {
                firstMove = false;
                StartCoroutine(MoveCooldown(0.4f));
            }
            else
            {
                StartCoroutine(MoveCooldown(0.05f));
            }
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        SetDisplay(true);
    }

    public void Disable()
    {
        if (grabSlot.occupied)
        {
            grabbedItemOriginalSlot.RestoreColor();
            grabSlot.Clear();
        }

        SetDisplay(false);
        gameObject.SetActive(false);
    }

    public void Position(bool isLootContainer, int newColumnIndex, int newRowIndex)
    {
        rowIndex = newRowIndex;
        columnIndex = newColumnIndex;
        inLootContainer = isLootContainer;
        
        if (isLootContainer)
            selectedSlot = lootSlots.Find(x => x.columnIndex == newColumnIndex && x.rowIndex == newRowIndex);
        else
            selectedSlot = inventorySlots.Find(x => x.columnIndex == newColumnIndex && x.rowIndex == newRowIndex);

        transform.position = new Vector2(selectedSlot.transform.position.x + cursorOffset.x, selectedSlot.transform.position.y + cursorOffset.y);
    }

    public void UseButtonPressed()
    {
        if (grabSlot.itemData)
            PlaceItem();
        else
            GrabItem();
    }

    private void GrabItem()
    {
        if (!selectedSlot.occupied)
            return;

        grabbedItemOriginalSlot = selectedSlot;
        selectedSlot.GrabItem();
        grabSlot.AddItem(selectedSlot.itemData, selectedSlot.stackSize);
    }

    private void PlaceItem()
    {
        if (selectedSlot.occupied)
        {
            if (selectedSlot.itemData == grabSlot.itemData)
            {
                selectedSlot.AddToStack(grabSlot.stackSize);
                grabbedItemOriginalSlot.Clear();
                grabSlot.Clear();
            }
            else
            {
                grabbedItemOriginalSlot.AddItem(selectedSlot.itemData, selectedSlot.stackSize);
                grabbedItemOriginalSlot.RestoreColor();
                selectedSlot.AddItem(grabSlot.itemData, grabSlot.stackSize);
                grabSlot.Clear();
            }
        }
        else
        {
            grabbedItemOriginalSlot.RestoreColor();
            grabbedItemOriginalSlot.Clear();
            selectedSlot.AddItem(grabSlot.itemData, grabSlot.stackSize);
            grabSlot.Clear();
        }
    }
    
    private void SetDisplay(bool show)
    {
        GetComponent<Image>().enabled = show;
        GetComponent<Collider2D>().enabled = show;
    }


    IEnumerator MoveCooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        shouldMove = true;
    }
}
