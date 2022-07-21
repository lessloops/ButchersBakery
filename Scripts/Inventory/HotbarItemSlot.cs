using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarItemSlot : MonoBehaviour
{
    public GameObject itemDisplay;
    public Text stackDisplay;

    public void ClearContents()
    {
        itemDisplay.SetActive(false);
    }

    public void UpdateContents(InventoryItemSlot newContents)
    {
        itemDisplay.GetComponent<Image>().sprite = newContents.itemDisplay.GetComponent<Image>().sprite;
        stackDisplay.text = newContents.stackSize.ToString();
        itemDisplay.SetActive(true);
    }
}
