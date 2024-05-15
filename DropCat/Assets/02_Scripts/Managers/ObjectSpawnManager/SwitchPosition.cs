using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPosition : MonoBehaviour
{
    [SerializeField] private GameObject groundHitBox = null;
    [SerializeField] private GameObject catHouse = null;
    private float groundCenterX = 0f;
    private float catHouseColliderExtentX = 0f;
    [SerializeField] private float offset = 10f;

    private float[] spawningColumnX = new float[8];
    private float spawningPositionY = 0f;

    private void Awake()
    {
        groundCenterX= groundHitBox.GetComponent<BoxCollider2D>().bounds.center.x;
        catHouseColliderExtentX = catHouse.GetComponent<BoxCollider2D>().size.x/2;
    }

    private void Start()
    {
        InitializeSpawningColumnX();
    }

    private void InitializeSpawningColumnX()
    {
        int halfIndex = spawningColumnX.Length / 2;
        for (int i = 0; i < halfIndex; i++)
        {
            int numberOfExtentsLeft = 2 * (halfIndex-i)-1;
            spawningColumnX[i] = groundCenterX - (catHouseColliderExtentX + offset) * numberOfExtentsLeft;
            int numberOfExtentsRight = 2 * i + 1;
            spawningColumnX[halfIndex + i] = groundCenterX + (catHouseColliderExtentX + offset) * numberOfExtentsRight;
        }
    }

    public Vector2 SwitchColumnX(int columnIndex, float spawningPositionY)
    {
        return new Vector2(spawningColumnX[columnIndex], spawningPositionY);
    }
}
