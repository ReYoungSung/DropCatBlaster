using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalDroneLaser : DroneLaser
{
    private float hitOffset = 5f;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        SetLaserLength();
        base.Update();
    }

    public virtual void SetLaserLength()
    {
        Vector2 laserOriginUp = new Vector2(transform.position.x, BoxCollider2D.bounds.max.y + LaserOffset);
        Vector2 laserOriginDown = new Vector2(transform.position.x, BoxCollider2D.bounds.min.y - LaserOffset);
        RaycastHit2D hitUp = Physics2D.Raycast(laserOriginUp, Vector2.up, LaserSensorLength, LaserHitLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(laserOriginDown, Vector2.down, LaserSensorLength, LaserHitLayer);

        if (hitUp)
        {
            LaserObj[0].SetPosition(0, new Vector3(0f, hitUp.point.y - transform.position.y, 0f));
            HitEffectObj[0].transform.position = hitUp.point;
            HitEffectObj[0].transform.localScale = new Vector3(3f, 3f, 1f);
            LaserBoxColliders2D[0].offset = new Vector2(0f, (hitUp.point.y - BoxCollider2D.bounds.min.y) / 2);
            LaserBoxColliders2D[0].size = new Vector2(LaserWidth, hitUp.point.y - BoxCollider2D.bounds.max.y);
        }
        else
        {
            Vector3 boundsUp = Vector2.up * LaserSensorLength;
            LaserObj[0].SetPosition(0, boundsUp);
            HitEffectObj[0].transform.position = transform.position + boundsUp;
            HitEffectObj[0].transform.localScale = new Vector3(1.4f, 4.1f, 1f);
            LaserBoxColliders2D[0].offset = new Vector2(0f, boundsUp.y / 2);
            LaserBoxColliders2D[0].size = new Vector2(LaserWidth, boundsUp.y);
        }

        if (hitDown)
        {
            LaserObj[1].SetPosition(1, new Vector3(0f, hitDown.point.y - transform.position.y, 0f));
            HitEffectObj[1].transform.position = hitDown.point;
            HitEffectObj[1].transform.localScale = new Vector3(3f, 3f, 1f);
            LaserBoxColliders2D[1].offset = new Vector2(0f, (hitDown.point.y - BoxCollider2D.bounds.max.y)/2);
            LaserBoxColliders2D[1].size = new Vector2(LaserWidth, BoxCollider2D.bounds.min.y - hitDown.point.y);
        }
        else
        {
            Vector3 boundsDown = Vector2.down * LaserSensorLength;
            LaserObj[1].SetPosition(1, boundsDown);
            HitEffectObj[1].transform.position = transform.position + boundsDown;
            HitEffectObj[1].transform.localScale = new Vector3(1.4f, 4.1f, 1f);
            LaserBoxColliders2D[1].offset = new Vector2(0f, boundsDown.y / 2);
            LaserBoxColliders2D[1].size = new Vector2(LaserWidth, boundsDown.y);
        }
    }
}
