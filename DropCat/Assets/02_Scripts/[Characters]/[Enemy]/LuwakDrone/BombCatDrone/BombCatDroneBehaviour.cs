using System.Collections;
using UnityEngine;
using Spine.Unity;
using CharacterBehaviour.Enemy;

public class BombCatDroneBehaviour : EnemyBehaviour
{
    private GameObject playerObj = null;
    private SkeletonAnimation skeletonAnimation = null;
    private Spine.TrackEntry trackEntry = null;
    private Coroutine coroutine = null;
    private EnemyEffects enemyEffects = null;

    [SerializeField] private GameObject damageCollisionObject = null;
    [SerializeField] private LuwakDroneAttribute bombCatData = null;

    protected override void Awake()
    {
        base.Awake();
        playerObj = GameObject.FindGameObjectWithTag("PLAYER_Character");
        skeletonAnimation = this.GetComponent<SkeletonAnimation>();
        enemyEffects = this.GetComponent<EnemyEffects>();
        health = bombCatData.health;
    }

    private void Start()
    {
        trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, "Moving", true);
    }

    private void Update()
    {
        DestructionProcess(enemyEffects);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (GameObject.ReferenceEquals(collision.gameObject, playerObj))
            {
                if (coroutine == null)
                {
                    coroutine = StartCoroutine(OnExplosion());
                }
            }
        }
    }

    private IEnumerator OnExplosion()
    {
        if (trackEntry.Animation.Name != "PrepToExplode")
        {
            trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, "PrepToExplode", true);
        }
        yield return new WaitForSeconds(skeletonAnimation.skeleton.Data.FindAnimation("PrepToExplode").Duration);
        enemyEffects.DestructionProcess(this.gameObject, false);
        Instantiate(damageCollisionObject,this.transform.position, Quaternion.identity);
        health = 0;
    }
}
