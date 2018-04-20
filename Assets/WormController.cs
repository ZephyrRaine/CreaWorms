using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour 
{
    Rigidbody2D rb;
    public float _speed;
    public float jumpSpeed;
    public float rotSpeed;
    public Transform weapon;

    public float speedForage;
    private void Start()
	{
        rb = GetComponent<Rigidbody2D>();
        weapon.GetComponentInChildren<DestroyTerrain>().creused += MoveTowards;
    }

    void MoveTowards(Vector3 pos)
    {
        Vector3 direction = (pos-transform.position).normalized;
        rb.AddForce((direction * speedForage * (direction.y+1f) * Time.deltaTime), ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

    }
    Vector3 pos;
        Vector2 screenPos;
    bool active;
    public void Activate(bool a)
    {
        rb.simulated = a;
        active = a;
    }
    private void Update()
    {
        if (active)
        {

            if (Input.GetMouseButtonDown(1))
            {
                Time.timeScale = 0.5f;
            }

            if (Input.GetMouseButtonUp(1))
            {
                Time.timeScale = 1f;
                Vector3 direction = (weapon.GetChild(0).position - transform.position).normalized;
                rb.AddForce((direction * jumpSpeed), ForceMode2D.Impulse);
            }

        }
    }
	private void FixedUpdate()
	{
        if (active)
        {

            Vector2 v = Vector2.right * Input.GetAxis("Horizontal") * _speed;

            // if(v.x > 0)
            //     transform.localScale = Vector3.one;
            // else if(v.x < 0)
            //     transform.localScale = new Vector3(-1f, 1f, 1f);
            rb.AddForce(v);

            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - weapon.position;
            difference.Normalize();
            float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.Euler(0f, 0f, rotation_z);
            weapon.rotation = Quaternion.RotateTowards(weapon.rotation, q, rotSpeed);
            // pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log(pos);
            // Vector3 vv = weapon.eulerAngles;
            // vv.z = Mathf.Clamp(vv.z, -15f, 45f);
            // weapon.eulerAngles = vv;

        }
    }

    private void LateUpdate()
    {
        if(active)
        {
            weapon.GetComponent<Rigidbody2D>().MovePosition(transform.position);
        }
    } 

}
