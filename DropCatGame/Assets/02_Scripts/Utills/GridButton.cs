using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridButton : MonoBehaviour
{
    [SerializeField] private GameObject gridPrefab;
    private GameObject grid;
    private bool gridIsOn = false;

    private void Awake()
    {
    }

    public void SwitchGrid()
    {
        if(!gridIsOn)
        {
            grid = Instantiate(gridPrefab);
            gridIsOn = true;
        }
        else
        {
            Destroy(grid.gameObject);
            gridIsOn = false;
        }
    }
}

