using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollisionObject : MonoBehaviour
{
    [SerializeField] private DamageCollisionObjectData damageCollisionObjectData = null;
    private Coroutine coroutine = null;

    private void Awake()
    {
        this.GetComponent<CircleCollider2D>().radius = damageCollisionObjectData.radius;
        if(coroutine == null)
        {
            coroutine = StartCoroutine(ActivateDamageCollision());
        }
    }

    private IEnumerator ActivateDamageCollision()
    {
        yield return new WaitForSeconds(damageCollisionObjectData.duration);
        Destroy(this.gameObject);
    }
}
