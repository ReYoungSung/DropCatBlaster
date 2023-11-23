using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterBehaviour.Enemy.CatResident
{
    public class CatResidentBehaviour : EnemyBehaviour
    {
        [SerializeField] private CatResidentAttribute catResidentData;
        private EnemyEffects enemyEffectsControl = null;
        private bool spawned = false;
        private AudioManager audioManager = null;

        protected override void Awake()
        {
            base.Awake();
            health = catResidentData.health;

            enemyEffectsControl = this.GetComponent<EnemyEffects>();
            audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        }

        private void Update()
        {
            DestructionProcess(enemyEffectsControl);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("DamageCollisionObject"))
            {
                health = 0;
                if (!spawned)
                {
                    spawned = true;
                }
                audioManager.PlaySFX("CatResidentPop_Sound");
            }
        }
    }
}
