using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudFloatingUp : MonoBehaviour
{
    private float cloudSpeed = 700f;
    private float destroyYHeight = 1432f;

    private void Update()
    {
        this.transform.Translate(Vector2.up * (cloudSpeed * Time.deltaTime));
        
        if(destroyYHeight < this.transform.position.y)
        {
            Destroy(this.gameObject);
        }
    }
}
