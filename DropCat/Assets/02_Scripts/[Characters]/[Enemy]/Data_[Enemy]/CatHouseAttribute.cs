using UnityEngine;

[CreateAssetMenu(fileName = "CatHouse Data", menuName = "Scriptable Object/CatHouse Data", order = int.MaxValue)]
public class CatHouseAttribute : ScriptableObject
{
    // Number of punches it can get
    public int health = 1;

    public float dropDistance = 20;
    public float dropTiming = 1;
    public float dropBreak = 1;

    public int damage = 5;

    public GameObject[] destructionParticleEffects;

    public string destructionSFX = "HousePunchImpact";

    public static CatHouseAttribute thisData;

    public CatHouseAttribute()
    {
        thisData = this;
    }
}