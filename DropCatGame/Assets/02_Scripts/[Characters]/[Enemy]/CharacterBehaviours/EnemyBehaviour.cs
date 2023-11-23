using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterBehaviour.Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        protected int health = 0;
        protected ObjectSpawnManager objectSpawnManager = null;

        protected virtual void Awake()
        {
            InitializeCachingReference();
        }

        private void InitializeCachingReference()
        {
            try
            {
                objectSpawnManager = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>();
            }
            catch
            {
                Debug.Log("Caching reference is not initialized : " + this.gameObject.name);
            }
        }

        public void DamageObject()
        {
            if (0 < health)
                --health;
        }

        public virtual void DestructionProcess(EnemyEffects enemyEffectsControl)
        {
            if (health <= 0)
            {
                    enemyEffectsControl.DestructionProcess(this.transform.position);
                    --objectSpawnManager.CatHouseCount;
                    Destroy(this.gameObject);
            }
        }

        public void DestructionProcess(GameObject spawningObj, EnemyEffects enemyEffectsControl)
        {
            if (health <= 0)
            {
                enemyEffectsControl.DestructionProcess(this.transform.position);
                if(spawningObj == null)
                {
                    --objectSpawnManager.CatHouseCount;
                }
                else
                {
                    if(spawningObj != null)
                    {
                        Instantiate(spawningObj, this.transform.position, Quaternion.identity);
                    }
                }
                Destroy(this.gameObject);
            }
            else
            {
                return;
            }
        }
    }
}
