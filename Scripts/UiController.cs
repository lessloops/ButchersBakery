using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public static UiController instance;
    public GameObject dialogueParent;
    public BuildModeCursor buildCursor;
    public InventoryCursor inventoryCursor;
    public GameObject inventoryUiObj;
    public GameObject hotbarUiObj;
    public GameObject lootUiObj;
    public GameObject dateTimeObj;
    public Image screenFill;

    InteractionInfo interactionInfo;
    Vector2 inventoryUiDefaultYPos = Vector2.zero;
    Vector2 inventoryUiLootYPos = new Vector2(0, -90f);
    int fadeDirection = 0;
    float fadeDuration = 0;

    private void Start()
    {
        instance = this;
        interactionInfo = GetComponent<InteractionInfo>();
    }

    private void Update()
    {
        if (fadeDirection == 0)
            return;

        if ((screenFill.color.a < 1 && fadeDirection == 1) || (screenFill.color.a > 0 && fadeDirection == -1))
        {
            float alphaVal = screenFill.color.a + (fadeDirection * fadeDuration * Time.deltaTime);
            screenFill.color = new Color(0, 0, 0, alphaVal);
        }
        else
        {
            fadeDirection = 0;
            fadeDuration = 0;
        }
    }

    public void FadeToBlack(float fadeTime)
    {
        fadeDuration = fadeTime;
        fadeDirection = 1;
    }

    public void FadeIn(float fadeTime)
    {
        fadeDuration = fadeTime;
        fadeDirection = -1;
    }

    public void ShowUi(string name)
    {
        switch (name)
        { 
            case "Live":
                dialogueParent.SetActive(false);
                buildCursor.Disable();
                inventoryCursor.Disable();
                hotbarUiObj.SetActive(true);
                dateTimeObj.SetActive(true);
                inventoryUiObj.SetActive(false);
                lootUiObj.SetActive(false);
                inventoryUiObj.GetComponent<RectTransform>().localPosition = inventoryUiDefaultYPos;
                break;

            case "Dialogue":
                interactionInfo.Hide();
                hotbarUiObj.SetActive(false);
                dateTimeObj.SetActive(false);
                dialogueParent.SetActive(true);
                inventoryUiObj.SetActive(false);
                lootUiObj.SetActive(false);
                break;

            case "Building":
                interactionInfo.Hide();
                hotbarUiObj.SetActive(false);
                dateTimeObj.SetActive(false);
                buildCursor.Enable();
                dialogueParent.SetActive(false);
                break;

            case "Inventory":
                interactionInfo.Hide();
                dialogueParent.SetActive(false);
                dateTimeObj.SetActive(false);
                hotbarUiObj.SetActive(false);
                inventoryUiObj.SetActive(true);
                inventoryCursor.Enable();
                break;

            case "Loot":
                interactionInfo.Hide();
                dialogueParent.SetActive(false);
                hotbarUiObj.SetActive(false);
                dateTimeObj.SetActive(false);
                lootUiObj.SetActive(true);
                inventoryUiObj.GetComponent<RectTransform>().localPosition = inventoryUiLootYPos;
                inventoryUiObj.SetActive(true);
                inventoryCursor.Enable();
                break;

            case "Placement":
                dialogueParent.SetActive(false);
                buildCursor.Disable();
                inventoryCursor.Disable();
                hotbarUiObj.SetActive(false);
                dateTimeObj.SetActive(true);
                inventoryUiObj.SetActive(false);
                lootUiObj.SetActive(false);
                break;

            case "Sleeping":
                interactionInfo.Hide();
                dialogueParent.SetActive(false);
                dateTimeObj.SetActive(false);
                hotbarUiObj.SetActive(false);
                lootUiObj.SetActive(false);
                inventoryUiObj.SetActive(false);
                inventoryCursor.Disable();
                break;
        }
    }
}
