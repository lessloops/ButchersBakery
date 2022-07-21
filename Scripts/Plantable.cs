using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantable : MonoBehaviour
{
    public int daysPerGrowth;
    public List<Sprite> growthSprites = new List<Sprite>();
    public GameObject harvestedCropPrefab;
    public int maxCrops = 1;
    public bool reharvestable;
    public int daysPerHarvest;

    int dayPlanted;
    int lastHarvest;
    int spriteIndex = -1;
    bool fullyGrown;

    public void Plant()
    {
        dayPlanted = TimeManager.instance.day;
        TimeManager.instance.AddPlant(this);
    }

    public void Grow()
    {
        if (fullyGrown)
            return;

        int currentDay = TimeManager.instance.day;

        if ((currentDay - dayPlanted) % daysPerGrowth == 0)
        {
            spriteIndex++;
            GetComponent<SpriteRenderer>().sprite = growthSprites[spriteIndex];

            if (spriteIndex == growthSprites.Count - 1)
            {
                fullyGrown = true;

                if (harvestedCropPrefab)
                {
                    gameObject.AddComponent<Harvestable>();
                    GetComponent<Harvestable>().Initialise(reharvestable, harvestedCropPrefab, maxCrops);
                }
            }
        }

        if (reharvestable)
        {
            if ((currentDay - lastHarvest) % daysPerHarvest == 0)
            {
                GetComponent<SpriteRenderer>().sprite = growthSprites[spriteIndex];
            }
        }
    }

    public void Harvest()
    {
        if (reharvestable)
        {
            Destroy(GetComponent<Harvestable>());
            lastHarvest = TimeManager.instance.day;
            spriteIndex--;
            GetComponent<SpriteRenderer>().sprite = growthSprites[spriteIndex];
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
