using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshMapBounds : MonoBehaviour {

    BoxCollider2D c;
    // Use this for initialization
    void Awake()
    {
        c = GetComponent<BoxCollider2D>();
    }

	public void UpdateBounds(SpriteRenderer sr)
	{
        c.size = sr.bounds.size;
        c.offset = sr.bounds.center;
    }

}
