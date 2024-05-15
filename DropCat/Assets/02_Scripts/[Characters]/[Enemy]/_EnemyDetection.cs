using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _EnemyDetection : MonoBehaviour
{
    private bool isDetected = false;
    public bool IsDetected { get { return isDetected; } set { isDetected = value; }}

    private bool crossHairSpawned = false;

    [SerializeField] private GameObject crossHairObj;
    private GameObject crossHair;
    private Animator crossHairAnimation;
    private Animator catHouseAnimator;

    // z ���� 0���� �����Ұ� 
    [SerializeField] private Vector3 crossHairSpawnOffset;

    private void Awake()
    {
        catHouseAnimator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        DisplayPlayerCrossHair();
    }

    private void DisplayPlayerCrossHair()
    {
        if (isDetected)
        {
            if(crossHairSpawned == false)
            {
                crossHair = Instantiate(crossHairObj, this.transform.position + crossHairSpawnOffset, Quaternion.identity);
                crossHair.transform.parent = this.gameObject.transform;
                crossHairSpawned = true;

                crossHairAnimation = crossHair.GetComponent<Animator>();
                if(crossHairAnimation)
                {
                    crossHairAnimation.SetBool("enemyDetected", true);
                }
            }
        }
        else
        {
            if(crossHair != null && crossHairSpawned)
            {
                Destroy(crossHair.gameObject);
                crossHairSpawned = false;
            }
        }
    }
}
