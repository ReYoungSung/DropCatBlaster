using UnityEngine;

[CreateAssetMenu(fileName = "CatResident Data", menuName = "Scriptable Object/CatResident Data", order = int.MaxValue)]
public class CatResidentAttribute : ScriptableObject
{
    // Number of punches it can get
    public int health = 1;
    public int damage = 1;
    public float maxSpeed = 10f;
    public float maxAccel = 10f;
    public float attackOffset = 10f;
    public GameObject[] destructionParticleEffects;
    public string destructionSFX = "CatResidentPunchImpact";
}
