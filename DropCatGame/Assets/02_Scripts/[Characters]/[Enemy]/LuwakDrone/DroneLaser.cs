using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class DroneLaser : MonoBehaviour
{
    private BoxCollider2D boxCollider2D = null;
    private BoxCollider2D[] laserBoxColliders2D = null;
    private LineRenderer[] laserObj = null;
    private GameObject[] hitEffectObj = null;
    private float laserSensorLength = 400f;
    private float laserOffset = 3f;
    private float laserWidth = 15f;
    [SerializeField] private LayerMask laserHitLayer;

    private float firingPrepareDuration = 0.3f;
    [SerializeField] private float firingDuration = 1f;
    [SerializeField] private float ceaseFireDelay = 2f;
    [SerializeField] private float addDelayToStart = 0f;
    private bool firstShot = true;
    private Coroutine laserRoutine = null;
    private SkeletonAnimation skeletonAnimation = null;
    private Spine.TrackEntry trackEntry = null;

    protected BoxCollider2D BoxCollider2D { get { return boxCollider2D; } }
    protected BoxCollider2D[] LaserBoxColliders2D { get { return laserBoxColliders2D; } set { laserBoxColliders2D = value; } }
    protected LineRenderer[] LaserObj { get { return laserObj; } }
    protected GameObject[] HitEffectObj { get { return hitEffectObj; } }
    protected float LaserSensorLength { get { return laserSensorLength; } }
    protected float LaserOffset { get { return laserOffset; } }
    protected float LaserWidth { get { return laserWidth; } }
    protected LayerMask LaserHitLayer { get { return laserHitLayer; } }

    public virtual void Awake()
    {
        boxCollider2D = this.GetComponent<BoxCollider2D>();
        laserObj = new LineRenderer[2];
        laserObj[0] = this.transform.GetChild(0).GetComponent<LineRenderer>();
        laserObj[1] = this.transform.GetChild(1).GetComponent<LineRenderer>();
        hitEffectObj = new GameObject[2];
        hitEffectObj[0] = this.transform.GetChild(2).gameObject;
        hitEffectObj[1] = this.transform.GetChild(3).gameObject;

        LaserBoxColliders2D = new BoxCollider2D[2];
        LaserBoxColliders2D[0] = this.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        LaserBoxColliders2D[1] = this.transform.GetChild(1).gameObject.GetComponent<BoxCollider2D>();

        skeletonAnimation = this.GetComponent<SkeletonAnimation>();
    }

    public virtual void Update()
    {
        if (laserRoutine == null)
        {
            laserRoutine = StartCoroutine(LaserFiringRoutine());
        }
    }

    private IEnumerator LaserFiringRoutine()
    {
        if (firstShot == true)
        {
            yield return new WaitForSeconds(addDelayToStart);
            firstShot = false;
        }
        while (this.gameObject.activeSelf)
        {
            PrepareFiringLaser();
            yield return new WaitForSeconds(firingPrepareDuration);
            FireLaser();
            yield return new WaitForSeconds(firingDuration);
            CeaseFireLaser();
            yield return new WaitForSeconds(ceaseFireDelay - firingPrepareDuration);
        }
    }

    private void FireLaser()
    {
        ToggleEffect(true);
        //SetLaserLength();
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, "ATTACKING", true);
    }

    private void PrepareFiringLaser()
    {
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, "ATTACKING_PREP", false);
    }

    private void CeaseFireLaser()
    {
        ToggleEffect(false);
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, "IDLING", true);
    }

    private void ToggleEffect(bool toggle)
    {
        for (int i = 0; i < 2; i++)
        {
            laserObj[i].gameObject.SetActive(toggle);
            hitEffectObj[i].SetActive(toggle);
        }
    }
}
