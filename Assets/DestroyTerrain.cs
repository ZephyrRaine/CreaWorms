using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyTerrain : MonoBehaviour {

	CircleCollider2D c;
	public bool permanent = false;
    public Action<Vector3> creused;
    ParticleSystem ps;
    private void Start()
	{
		c = GetComponent<CircleCollider2D>();
        ps = GetComponentInChildren<ParticleSystem>();
    }

	public static DestroyTerrain Instantiate(Vector3 position, float radius)
	{
        DestroyTerrain dt = new GameObject("TempCC", typeof(CircleCollider2D), typeof(DestroyTerrain), typeof(Rigidbody2D)).GetComponent<DestroyTerrain>();
        dt.transform.position = position;
        CircleCollider2D cc = dt.GetComponent<CircleCollider2D>();
		cc.radius = radius;
		cc.isTrigger  = true;
        Rigidbody2D rb = dt.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        return dt;
    }

    private void OnTriggerStay2D(Collider2D other)
	{
        if (other is PolygonCollider2D && ((!permanent || (permanent && Input.GetMouseButton(0)))))
        {
            WormsTerrain terrain = other.GetComponent<WormsTerrain>();
            if (terrain != null)
            {
                terrain.DestroyGround(c);
                if (creused != null)
                    creused.Invoke(transform.position);
				if(ps != null)
	                ps.Play();
                if (!permanent)
                    Destroy(gameObject);
            }
        }
    }
}
