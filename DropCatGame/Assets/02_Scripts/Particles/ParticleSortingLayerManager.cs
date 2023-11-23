using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSortingLayerManager : MonoBehaviour
{
    [SerializeField] string sortingLayerName;
    ParticleSystem particleSys;

    private void Awake()
    {
        particleSys = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        particleSys.GetComponent<Renderer>().sortingLayerName = sortingLayerName;
    }
}
