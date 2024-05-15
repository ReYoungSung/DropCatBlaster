using UnityEngine;

[CreateAssetMenu(fileName = "LuwakDrone Data", menuName = "Scriptable Object/LuwakDrone Data", order = int.MaxValue)]
public class LuwakDroneAttribute : ScriptableObject
{
    // Number of punches it can get
    public int health = 1;
    public int damage = 1;
    public float maxSpeed = 20f;
    public float maxAccel = 10f;
    public GameObject[] destructionParticleEffects;
    public string destructionSFX = "LuwakDronePunchImpact";

    public static LuwakDroneAttribute thisData;

    public LuwakDroneAttribute()
    {
        thisData = this;
    }
}
