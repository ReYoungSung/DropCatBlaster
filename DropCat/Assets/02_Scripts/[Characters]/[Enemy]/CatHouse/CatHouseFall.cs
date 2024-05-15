using System;
using UnityEngine;
using Spine.Unity;

namespace CharacterBehaviour.Enemy.CatHouse
{
    public class CatHouseFall : MonoBehaviour
    {
        private float colliderHalfHeight = 0;
        private Rigidbody2D rigidBody2D;
        [SerializeField] private LayerMask landingLayer;
        private float dropDist;
        private LTDescr leanTween = null;
        private CatHouseBehaviour catHouseBehaviour = null;
        private HoppableObject hoppableObj = null;
        private BoxCollider2D boxCollider2D = null;

        private void Awake()
        {
            boxCollider2D = this.GetComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = true;
            colliderHalfHeight = boxCollider2D.bounds.extents.y;
            rigidBody2D = this.GetComponent<Rigidbody2D>();
            leanTween = new LTDescr();
            catHouseBehaviour = this.GetComponent<CatHouseBehaviour>();
            hoppableObj = this.GetComponent<HoppableObject>();
        }

        private void Update()
        {
            DisplaySensorGizmo(this.transform.position, Vector2.down, dropDist);
        }

        public void CatHouseDrop(float maxDropDistance, float dropOnceDur)
        {
            //If the CatHouse needs to land on a ground
            /* 
            float dropDistance = ComputeDropDistance(maxDropDistance);
            if (dropDistance < maxDropDistance)
            {
                catHouseBehaviour.HasCollided = true;
                hoppableObj.enabled = false;
            }
            dropDist = dropDistance;
            */

            LeanTween.init();
            leanTween = LeanTween.moveY(this.gameObject, transform.position.y - maxDropDistance, dropOnceDur).setEaseInOutBack();
        }

        public void CatHouseFreeFalls()
        {
            rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
        }

        private float ComputeDropDistance(float maxDropDist)
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)this.transform.position + Vector2.down * colliderHalfHeight, Vector2.down, maxDropDist, landingLayer);
            if (hit)
            {
                if (HasReachedGround(hit))
                {
                    if (hit.transform.GetComponent<CatHouseBehaviour>().HasCollided)
                    {
                        return hit.distance;
                    }
                    else
                    {
                        return maxDropDist;
                    }
                }
                else
                {
                    return maxDropDist;
                }
            }
            else
            {
                return maxDropDist;
            }
        }

        private bool HasReachedGround(RaycastHit2D raycastHit2D)
        {
            return raycastHit2D.transform.gameObject.CompareTag("ENEMY_CatHouse")
                || raycastHit2D.transform.gameObject.CompareTag("GROUNDOBJ");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (HasLanded(collision))
            {
                this.GetComponent<CatHouseBehaviour>().HasCollided = true;
            }
        }

        private bool HasLanded(Collision2D colli)
        {
            return colli.gameObject.CompareTag("GROUNDOBJ") || colli.gameObject.CompareTag("ENEMY_CatHouse");
        }

        private void DisplaySensorGizmo(Vector2 origin, Vector2 direction, float attackDistance)
        {
            Debug.DrawLine(origin, origin + direction * attackDistance);
        }
    }
}