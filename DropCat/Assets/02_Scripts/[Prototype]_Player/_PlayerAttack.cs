/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float angleScope = 30;
    private float attackAngle = -1;
    private Vector2 preInput;    //공격을 한번 누를때 한번만 인식해야 함

    private _PlayerMovement playerMovement;
    private Animator attackAnimator;
    [SerializeField] private ParticleSystem explosion;

    private void Awake()
    {
        playerMovement = this.GetComponentInParent<_PlayerMovement>();
        attackAnimator = this.GetComponentInParent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        float angle = GetAngle(this.transform.position ,other.transform.position);
        if (attackAngle != -1)   //tag 비교시 가해지는 부하를 줄일 수 있음.
        {
            if(other.CompareTag("Enemy"))
            {
                if (AngleWithinAttackArea(angle))
                {
                    ActivateHitParticles(other.transform);
                    DestroyEnemy(other.transform);
                }
            }
            // 다른 오브젝트들 (Props 등)이 범위에 들어왔을때 if 문 추가
            else//아무것도 range에 없을때
            {
                Debug.Log("공격실패");
            }
        }
    }

    private float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 heading = end - start;
        float angle =  Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        if (angle < 0f) angle += 360f;
        return angle;
    }

    public void GetAttackAngle(Vector2 attackInput)
    {
        if(attackInput != Vector2.zero && preInput == Vector2.zero)
            attackAngle = GetAngle(Vector2.zero, attackInput);
        else
            attackAngle = -1;
        preInput = attackInput;
    }

    private bool AngleWithinAttackArea(float angle)
    {
        return attackAngle - angleScope <= angle && angle <= attackAngle + angleScope;
    }

    private void DestroyEnemy(Transform other)
    {
        this.gameObject.transform.parent.position = other.transform.position;
        Destroy(other.gameObject);
    }

    private void ActivateHitParticles(Transform hitTransform)
    {
        GameObject explosionParticle = Instantiate(explosion.gameObject, hitTransform.position, Quaternion.identity);
        Destroy(explosionParticle.gameObject, 50f);
    }

    public void BasicAttackHorizontal(Vector2 attackInput)
    {
        float speedBuffer = playerMovement.Speed;
        if (attackInput != Vector2.zero)
        {
            playerMovement.Speed = 0f;
            playerMovement.FlipCharacter(attackInput);
            if (attackInput.x < -0.01)
            {

            }
            else if (attackInput.x > 0.01)
            {

            }
        }
        else
        {
            attackAnimator.SetBool("isAttacking", false);
        }
    }

    private void BasicAttackHorizontalRoutine(Vector2 dir)
    {
        const float dashPower = 5f;
        Vector2 pos = this.transform.parent.position;
        this.transform.parent.position = pos + new Vector2(dashPower, 0) * dir;
    }
}
*/