using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    Color[] startingColors;
    string[] sortingLayers;
    SpriteRenderer[] srs;
    Transform lifterTransform;
    Vector2 startingPosition;
    BoxCollider2D col;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();

        if (GetComponent<SpriteRenderer>())
            srs = new SpriteRenderer[] { GetComponent<SpriteRenderer>() };
        else
            srs = GetComponentsInChildren<SpriteRenderer>();

        startingColors = new Color[srs.Length];
        sortingLayers = new string[srs.Length];

        for (int i = 0; i < srs.Length; i++)
        {
            startingColors[i] = srs[i].color;
            sortingLayers[i] = srs[i].sortingLayerName;
        }
    }

    private void Update()
    {
        if (lifterTransform == null)
            return;

        float x = (float)Math.Round(lifterTransform.position.x * 2) / 2;
        float y = (float)Math.Round(lifterTransform.position.y * 2) / 2;
        transform.position = new Vector2(x, y);
    }

    public void Lift(Transform cursor)
    {
        lifterTransform = cursor;
        startingPosition = transform.position;
        col.enabled = false;
        RaiseSortingLayer();
    }

    public void Drop()
    {
        lifterTransform = null;
        startingPosition = Vector2.zero;
        col.enabled = true;

        RestoreDefaults();
    }

    public void Return()
    {
        lifterTransform = null;
        transform.position = startingPosition;
        col.enabled = true;

        RestoreDefaults();
    }

    public void SetColors(Color newColor)
    {
        for (int i = 0; i < srs.Length; i++)
        {
            srs[i].color = newColor;
        }
    }

    private void RaiseSortingLayer()
    {
        for (int i = 0; i < srs.Length; i++)
        {
            srs[i].sortingLayerName = "WorldUI";
        }
    }

    private void RestoreDefaults()
    {
        for (int i = 0; i < srs.Length; i++)
        {
            srs[i].color = startingColors[i];
            srs[i].sortingLayerName = sortingLayers[i];
        }
    }
}
