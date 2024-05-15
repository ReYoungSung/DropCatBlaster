using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(EdgeCollider2D))]
public class RigidBodyInsideCollider : MonoBehaviour
{
    private void Awake()
    {
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
        Vector2[] points = poly.points;
        EdgeCollider2D edge = gameObject.GetComponent<EdgeCollider2D>();
        edge.points = points;
        Destroy(poly);
    }
}
