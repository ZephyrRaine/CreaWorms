using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWorm : MonoBehaviour {

	public Transform tr;
	TMPro.TMP_Text t;
	private void Start()
	{
		t = GetComponent<TMPro.TMP_Text>();
	}
	private void LateUpdate()
	{
		transform.position = tr.position + Vector3.right * 0.25f;
		t.text = "<-- " + (int)transform.position.y + "m";
	}
}
