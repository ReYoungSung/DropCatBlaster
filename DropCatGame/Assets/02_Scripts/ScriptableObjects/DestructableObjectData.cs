using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestructableObjectData", menuName = "Scriptable Object/DestructableObjectData", order = int.MaxValue)]
public class DestructableObjectData : ScriptableObject
{
    // Number of punches it can get
    public int durability = 100;
}
