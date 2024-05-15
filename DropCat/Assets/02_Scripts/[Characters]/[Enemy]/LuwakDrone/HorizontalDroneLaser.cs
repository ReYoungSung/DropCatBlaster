using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalDroneLaser : DroneLaser
{
    private float boundsLeft = 0f;
    private float boundsRight = 0f;
    private float boundsOffset = 12f;

    private void Start()
    {
        GroundCollider2D groundCollider2D = GameObject.Find("[GroundCollider2D]").GetComponent<GroundCollider2D>();
        boundsLeft = groundCollider2D.BoundsLeft;
        boundsRight = groundCollider2D.BoundsRight;
    }

    public override void Update()
    {
        SetLaserLength();
        base.Update();
    }

    private void SetLaserLength()
    {
        Vector2 laserOriginLeft = new Vector2(BoxCollider2D.bounds.min.x - LaserOffset, transform.position.y);
        Vector2 laserOriginRight = new Vector2(BoxCollider2D.bounds.max.x + LaserOffset, transform.position.y);
        RaycastHit2D hitLeft = Physics2D.Raycast(laserOriginLeft, Vector2.left, LaserSensorLength, LaserHitLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(laserOriginRight, Vector2.right, LaserSensorLength, LaserHitLayer);

        if (hitLeft)
        {
            LaserObj[0].SetPosition(0, new Vector3(0f, transform.position.x - hitLeft.point.x, 0f));
            HitEffectObj[0].transform.position = hitLeft.point;
            LaserBoxColliders2D[0].offset = new Vector2(0f, (transform.position.x - hitLeft.point.x) / 2f);
            LaserBoxColliders2D[0].size = new Vector2(LaserWidth, transform.position.x - hitLeft.point.x);
        }
        else
        {
            LaserObj[0].SetPosition(0, new Vector3(0f, transform.position.x - boundsLeft + boundsOffset, 0f));
            HitEffectObj[0].transform.position = new Vector2(boundsLeft - boundsOffset, this.transform.position.y);
            LaserBoxColliders2D[0].offset = new Vector2(0f, (transform.position.x - boundsLeft + boundsOffset) / 2);
            LaserBoxColliders2D[0].size = new Vector2(LaserWidth, transform.position.x - boundsLeft + boundsOffset);
        }

        if (hitRight)
        {
            LaserObj[1].SetPosition(1, new Vector3(0f, transform.position.x - hitRight.point.x, 0f));
            HitEffectObj[1].transform.position = hitRight.point;
            LaserBoxColliders2D[1].offset = new Vector2(0f, (transform.position.x - hitRight.point.x) / 2);
            LaserBoxColliders2D[1].size = new Vector2(LaserWidth, hitRight.point.x - transform.position.x);
        }
        else
        {
            LaserObj[1].SetPosition(1, new Vector3(0f, transform.position.x - boundsRight - boundsOffset, 0f));
            HitEffectObj[1].transform.position = new Vector2(boundsRight + boundsOffset, this.transform.position.y);
            LaserBoxColliders2D[1].offset = new Vector2(0f, (transform.position.x - boundsRight - boundsOffset) / 2);
            LaserBoxColliders2D[1].size = new Vector2(LaserWidth, boundsRight - transform.position.x + boundsOffset);
        }
    }
}
