using UnityEngine;

public class CatSpaceshipHatchBehaviour : MonoBehaviour
{
    private DestructableObjectData hatchAttribute = null;
    private int durability = 0;
    public int Durability { get { return durability; } set { durability = value; } }

    private void Awake()
    {
        durability = hatchAttribute.durability;
    }

    private void Update()
    {
        MonitorDestroy();
    }

    public void DamageObject()
    {
        if (0 < durability)
            --durability;
        else if(durability == 0)
        {
            durability = 0;
        }
    }

    private void MonitorDestroy()
    {
        if (durability <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            return;
        }
    }
}
