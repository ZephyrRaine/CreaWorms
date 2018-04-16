using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormsTerrain : MonoBehaviour {

SpriteRenderer sr;
Collider2D col;
	// Use this for initialization
	void Start () 
	{
		sr = GetComponent<SpriteRenderer>();	
		col = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 wPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D c = Physics2D.Raycast(wPos,wPos, 1f);
		if(c.collider != null)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Texture2D d = sr.sprite.texture;
				Texture2D newTex = new Texture2D(d.width, d.height, d.format, false);
				newTex.wrapMode = TextureWrapMode.Clamp;
				newTex.alphaIsTransparency = true;
				
	            newTex.SetPixels(d.GetPixels());
    	        newTex.Apply();
				sr.sprite = Sprite.Create(newTex, new Rect(0.0f, 0.0f, newTex.width, newTex.height), sr.sprite.pivot, sr.sprite.pixelsPerUnit);
			//	sr.material.mainTexture = newTex;
				
				// newTex.SetPixel((int)(localPos.x/sr.sprite.pixelsPerUnit),(int)(localPos.y/sr.sprite.pixelsPerUnit), Color.red);
				// for(int i = 0; i < p.Length; i++)
				// {
				// 	if(p[i].a != 0)
				// 		p[i] = Color.red;	 
				// }
				// p[index] =  Color.red;
				
				// newTex.SetPixels32(p);
				 
				Debug.Log(c.point);
				Destroy(GetComponent<PolygonCollider2D>());
				gameObject.AddComponent<PolygonCollider2D>();
			}
		}
		
	}

	int GetPixel(int x, int y, int largeur)
	{
		return y * largeur + x;
	}

	Vector2 GetPixel(int index, int largeur)
	{
		return new Vector2(index%largeur, index/largeur);
	}

	

	IEnumerator RefreshCollider(Collider2D col)
     {
         // Remember to call this with StartCoroutine
         // This will make a collider that will trigger 
         // it's OnEnterState again.
 
         col.enabled = false;
 
         // Wait a frame so the collider can update 
         // it's status to false 
         yield return null;
 
         // Enable
         col.enabled = true;
 
         yield return null;
 
         // Force an update to the collider logic by nudging the 
         // the transform but will ultimately not move the object
         col.transform.localPosition += new Vector3(0.01f, 0, 0);
         col.transform.localPosition += new Vector3(-0.01f, 0, 0);
     }
}
