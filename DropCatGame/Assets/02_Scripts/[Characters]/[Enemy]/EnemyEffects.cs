using UnityEngine;
using CharacterManager;
using CharacterBehaviour.Attack;

public class EnemyEffects : MonoBehaviour
{
    private PlayerBasicAttack basicAttackControl = null;
    private CombatVibration combatVibration;

    private float slowDownFactor = 0.05f;
    private const int slowDownPunchCount = 5;

    private int punchCount = 0;

    private VFXSpawningManager hitEffectManager = null;
    private AudioManager audioManager = null;
    private const string damageCollisionTagName = "DamageCollisionObject";
    private bool isDestroyed = false;

    private void Awake()
    {
        basicAttackControl = GameObject.Find("[Player]").transform.GetChild(0).GetComponent<PlayerBasicAttack>();
        combatVibration = this.GetComponent<CombatVibration>();
        hitEffectManager = this.GetComponent<VFXSpawningManager>();
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
    }

    public void DestructionProcess(Vector2 effectSpot, bool slowMotion = true)
    {
        if(!isDestroyed)
        {
            ManageEnemyDestructionEffects(effectSpot, slowMotion);
            isDestroyed = true;
        }
    }

    private void ManageEnemyDestructionEffects(Vector2 effectSpot, bool slowMo = true)
    {
        if (basicAttackControl.CurrentPunchingEnemy == EnemyType.CatHouse || basicAttackControl.CurrentPunchingEnemy == EnemyType.BombCat)
        {
            combatVibration.EnemyDestruction();
            //if (slowMo)
              //  SlowMotion.instance.ActivateSlowMotion(0.3f, 1f);
        }
        audioManager.PlaySFX(DetermineEnemyDestructionSound());
        ActivateDestructionParticles(effectSpot);
    }

    public void DestructionProcess(GameObject thisGameObj, bool slowMotion = true)
    {
        ManageEnemyDestructionEffects(thisGameObj, thisGameObj.transform.position, slowMotion);
    }

    private void ManageEnemyDestructionEffects(GameObject thisGameObj, Vector2 effectSpot, bool slowMo = true)
    {
        combatVibration.EnemyDestruction();
        //if (slowMo)
          // SlowMotion.instance.ActivateSlowMotion(0.3f, 1f);
        audioManager.PlaySFX(DetermineEnemyDestructionSound(thisGameObj));
        ActivateDestructionParticles(effectSpot);
    }

    private void ActivateDestructionParticles(Vector2 spot)
    {
        hitEffectManager.BatchActivateHitEffect(spot);
    }

    private void ActivateDestructionParticles(GameObject hitParticleObj, Vector2 spot)
    {
        Instantiate(hitParticleObj, spot, Quaternion.identity);
    }

    private void ActivateDestructionParticles(GameObject hitParticleObj, Vector2 spot, float time)
    {
        GameObject explosionParticle = Instantiate(hitParticleObj, spot, Quaternion.identity);
        Destroy(explosionParticle.gameObject, time);
    }

    public void ManageEnemyHitEffects()
    {
        combatVibration.EnemyPunch();
    }

    private string DetermineEnemyDestructionSound()
    {
        string punchingSound = null;
        if(basicAttackControl.CurrentPunchingEnemy == EnemyType.CatHouse)
        {
            punchingSound = CatHouseAttribute.thisData.destructionSFX;

        }
        else if(basicAttackControl.CurrentPunchingEnemy == EnemyType.CatResident)
        {
            punchingSound = "CatResidentPop_Sound";
        }
        else if (basicAttackControl.CurrentPunchingEnemy == EnemyType.TutorialCatHouse)
        {
            punchingSound = "HousePunchImpact";
        }
        else if (basicAttackControl.CurrentPunchingEnemy == EnemyType.BombCat)
        {
            punchingSound = "BombCatDroneExplosion_01";
        }
        return punchingSound;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(damageCollisionTagName))
        {
            DestructionProcess(collision, this.transform.position, false);
        }
    }

    public void DestructionProcess(Collider2D collision, Vector2 effectSpot, bool slowMotion = true)
    {
        ManageEnemyDestructionEffects(collision, effectSpot, slowMotion);
    }


    private void ManageEnemyDestructionEffects(Collider2D collision, Vector2 effectSpot, bool slowMo = true)
    {
        if (collision.CompareTag(damageCollisionTagName))
        {
            combatVibration.EnemyDestruction();
            //if (slowMo)
              //  SlowMotion.instance.ActivateSlowMotion(0.3f, 1f);
        }
        audioManager.PlaySFX(DetermineEnemyDestructionSound(this.gameObject));
        ActivateDestructionParticles(effectSpot);
    }

    private string DetermineEnemyDestructionSound(GameObject thisObject)
    {
        string punchingSound = null;
        if (thisObject.CompareTag("ENEMY_CatHouse"))
        {
            punchingSound = CatHouseAttribute.thisData.destructionSFX;

        }
        else if (thisObject.CompareTag("ENEMY_CatResident"))
        {
            punchingSound = "CatResidentPop_Sound";
        }
        else if (thisObject.CompareTag("ENEMY_BombCatDrone"))
        {
            punchingSound = "BombCatDroneExplosion_01";
        }
        else if (thisObject.CompareTag("ENEMY_TutorialCatHouse"))
        {
            punchingSound = "HousePunchImpact";
        }
        return punchingSound;
    }
}
