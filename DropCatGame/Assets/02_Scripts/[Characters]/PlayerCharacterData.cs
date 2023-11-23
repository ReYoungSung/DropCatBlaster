using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharacterData", menuName = "Scriptable Object/Character Data/[Player]", order = int.MaxValue)]

public class PlayerCharacterData : ScriptableObject
{
    public GameObject playerCharacter;
    public Vector2 initialPosition = Vector2.zero;
    public float moveXSpeed = 0f;
    public float jumpHeight = 0f;
    public int maxJumpCount = 0;
    public float teleportDownDistance = 70f;
}
