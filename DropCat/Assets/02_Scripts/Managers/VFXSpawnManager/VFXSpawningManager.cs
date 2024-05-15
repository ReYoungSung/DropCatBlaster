using UnityEngine;
using CartoonFX;

public class VFXSpawningManager : MonoBehaviour
{
    [SerializeField] private GameObject[] VFXArray;
    public int GetArrayLength { get { return VFXArray.Length; } }

    public void BatchActivateHitEffect(Vector2 position)
    {
        foreach(GameObject vFX in VFXArray)
            Instantiate(vFX, position, Quaternion.identity);
    }

    public void ActivateHitEffect(Vector2 position, int index)
    {
        Instantiate(VFXArray[index], position, Quaternion.identity);
    }
}
