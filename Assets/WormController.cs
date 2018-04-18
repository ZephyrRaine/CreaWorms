using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour 
{
    Rigidbody2D rb;
    public float _speed;
    public float jumpSpeed;
    public float rotSpeed;
    private void Start()
	{
        rb = GetComponent<Rigidbody2D>();
    }
	private void FixedUpdate()
	{
        Vector2 v = Vector2.right * Input.GetAxis("Horizontal") * _speed;
		
		if(v.x > 0)
            transform.localScale = Vector3.one;
		else if(v.x < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        rb.AddForce(v);

        float r = Input.GetAxis("Vertical") * rotSpeed;
        transform.GetChild(0).Rotate(Vector3.forward, r);

        Vector3 vv = transform.GetChild(0).eulerAngles;
        vv.z = Mathf.Clamp(vv.z, -15f, 45f);
        // transform.GetChild(0).eulerAngles = vv;

        if(Input.GetKeyDown(KeyCode.Space))
		{
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }

		if(Input.GetAxis("Fire") > 0f)
		{
			
		}
    }

}
