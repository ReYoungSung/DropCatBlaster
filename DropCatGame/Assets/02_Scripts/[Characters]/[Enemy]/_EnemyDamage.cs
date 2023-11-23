using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _EnemyDamage : MonoBehaviour
{
    private Renderer enemyColor;

    // Start is called before the first frame update
    void Start()
    {
        enemyColor = gameObject.transform.GetComponent<Renderer>();
    }

    private void OnDestroy()
    {
        Debug.Log("Àû Ã³Ä¡");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            enemyColor.material.color = Color.blue;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            enemyColor.material.color = Color.white;
        }
    }
}
