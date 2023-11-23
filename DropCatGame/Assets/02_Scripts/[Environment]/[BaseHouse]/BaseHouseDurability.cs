using UnityEngine;

public class BaseHouseDurability : MonoBehaviour
{
    [SerializeField] private DestructableObjectData destructableAttribute;
    private int maxDurability;
    public int MaxDurability { get { return maxDurability; } }
    private int durability;
    public int Durability { get { return durability; } }

    public static BaseHouseDurability instance;
    private void Awake()
    {
        maxDurability = destructableAttribute.durability;
        durability = maxDurability;
        instance = this;
    }

    private void Start()
    {
        ResetDurability();
    }

    public void ResetDurability()
    {
        durability = maxDurability;
    }

    private void Update()
    {
        AnnounceGameOver();
    }

    public void DamageObject(int damage)
    {
        if (0 < durability)
            durability -= damage;
        else
            durability = 0;
    }

    private void AnnounceGameOver()
    {
        if (durability <= 0)
        {
            durability = 0;
            GameOverEventManager.current.GameOverTriggerEnter();
        }
        else
            return;
    }
}
