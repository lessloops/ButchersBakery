using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : Interactable
{
    bool reharvestable;
    GameObject harvestedCropPrefab;
    int maxCrops;

    public void Initialise(bool canReharvest, GameObject cropPrefab, int newMaxCrops)
    {
        reharvestable = canReharvest;
        harvestedCropPrefab = cropPrefab;
        maxCrops = newMaxCrops;
    }

    public override void Interact(Transform player)
    {
        int spawnAmount = Random.Range(1, maxCrops + 1);

        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject newCrop = Instantiate(harvestedCropPrefab, transform.position, Quaternion.identity);
            newCrop.GetComponent<WorldItem>().Spawn(transform.position);
        }
        
        GetComponent<Plantable>().Harvest();
    }
    
    public override string ActionText()
    {
        return "Harvest";
    }
}
