using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropEnabler : MonoBehaviour
{
    [SerializeField] GameObject[] targetObj;

    public GameObject[] TargetObj { get { return targetObj; } }

    public void EnableTargetObj()
    {
        foreach(GameObject obj in targetObj)
        {
            if (obj.activeSelf == true)
                obj.SetActive(false);
            else
                obj.SetActive(true);
        }
    }
}
