using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;

    //In Seconds
    private float drawLineDuration = 1.5f;

    public Grid(int width, int height, float cellSize, Vector3 fontOffset)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];

        for(int x = -width/2; x < width/2; x++)
        {
            for (int y = 0; y < height; y++)
            {
                string gridPos = "(" + x + ", " + y + ")";
                Vector3 worldPos = GetWorldPosition(x, y);
                TextUtills.CreateWorldText(gridPos, null, GetFontPosition(worldPos, fontOffset), 20, Color.black, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, drawLineDuration);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, drawLineDuration);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black, drawLineDuration);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black, drawLineDuration);
    }

    private Vector3 GetFontPosition(Vector3 worldPos, Vector3 offset)
    {
        return worldPos + offset;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }
}

