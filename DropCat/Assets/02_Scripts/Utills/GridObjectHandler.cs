using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectHandler : MonoBehaviour
{
    [SerializeField] private Grid grid = null;
    [SerializeField] int width = 45;
    [SerializeField] int height = 45;
    [SerializeField] float cellSize = 10f;
    [SerializeField] Vector2 textOffset = new Vector3(0, 1.8f, 0);

    private void Awake()
    {
        grid = new Grid(width, height, cellSize, textOffset);
    }

    private void OnDestroy()
    {
        TextUtills.DestoryWorldText();
    }
}
