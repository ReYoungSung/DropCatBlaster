using UnityEngine;

[CreateAssetMenu(fileName = "Damage Collision Object Data", menuName = "Scriptable Object/Damage Collision Object Data", order = int.MaxValue)]

public class DamageCollisionObjectData : ScriptableObject
{
    public float radius = 100f;
    public float duration = 0.01f;
}
