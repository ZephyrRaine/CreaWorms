using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyTerrain : MonoBehaviour {

	CircleCollider2D c;
	bool available = true;
    public Action<Vector3> creused;

    private void Start()
	{
		c = GetComponent<CircleCollider2D>();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.K))
		{
			available = true;
		}
	}
	private void OnTriggerStay2D(Collider2D other)
	{
		if(Input.GetMouseButton(0))
		{
			WormsTerrain terrain = other.GetComponent<WormsTerrain>();
			if(terrain != null)
			{
				terrain.DestroyGround(c);
				if(creused != null)
					creused.Invoke(transform.position);
			}
		}
		
	}
}
