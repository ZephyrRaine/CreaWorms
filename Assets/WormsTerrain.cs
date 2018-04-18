﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class WormsTerrain : MonoBehaviour {

	private SpriteRenderer sr;
	private float widthWorld, heightWorld;
	private int widthPixel, heightPixel;
	private Color transp;
	public CustomRenderTexture customRenderTexture;
	public Texture2D[] baseTerrain;
	public Material material;

    public UnityEvent OnGenerated;
    // Start() de GroundController
    void Start()
	{
		Generate();
	}

	void Generate()
	{
		Debug.Log("StartingGeneration");
		// customRenderTexture = new CustomRenderTexture(baseTerrain[0].width, baseTerrain[0].height);
		// customRenderTexture.material = material;
		customRenderTexture.material.SetTexture("_Tex", baseTerrain[Random.Range(0, baseTerrain.Length)]);
		customRenderTexture.material.SetFloat("_noiseResolutionBlack", Random.Range(0.01f, 0.05f));
		customRenderTexture.material.SetFloat("_noiseResolution", Random.Range(0.1f, 0.2f));
		customRenderTexture.Update();
     	RenderTexture currentActiveRT = RenderTexture.active;
     	// Set the supplied RenderTexture as the active one
     	RenderTexture.active = customRenderTexture;
     	// // Create a new Texture2D and read the RenderTexture image into it
		Texture2D readTexture = new Texture2D(customRenderTexture.width, customRenderTexture.height, TextureFormat.ARGB32, false);
        //  // Restorie previously active render texture
        readTexture.wrapMode = TextureWrapMode.Clamp;
        readTexture.ReadPixels(new Rect(0,0, customRenderTexture.width, customRenderTexture.height), 0, 0);

		RenderTexture.active = currentActiveRT;
		List<Vector2> edgePoints = LoadTerrainTypePoints(readTexture);
		FloodFill(readTexture, Vector2.zero, new Stack<Vector2>(edgePoints), new Color(0,0,0,0));
		Color32[] pixels = readTexture.GetPixels32();
		for(int i = 0; i < pixels.Length; i++)
		{
			if(pixels[i] == new Color(1,1,0,1))
				pixels[i] = new Color(0,0,0,0);
		}
		readTexture.SetPixels32(pixels);
		readTexture = TextureFilter.Convolution(readTexture, TextureFilter.GaussianKernel(0.4089642f, 2, true), 2);
		readTexture = TextureFilter.Convolution(readTexture, TextureFilter.DILATION_KERNEL, 2);
		readTexture.Apply();
		sr = GetComponent<SpriteRenderer>();

		// sr : variavel global de GroundController, ref para o SpriteRenderer de Ground
		Texture2D tex = readTexture;
		// Resources.Load("nome_do_arquivo") carrega um arquivo localizado
		// em Assets/Resources
		Texture2D tex_clone = (Texture2D) Instantiate(tex);
		// Criamos uma Texture2D clone de tex para nao alterarmos a imagem original
		sr.sprite = Sprite.Create(tex_clone,
		                          new Rect(0f, 0f, tex_clone.width, tex_clone.height),
		                          new Vector2(0.5f, 0.5f), 50f);


		InitSpriteDimensions();
		if(OnGenerated != null)
            OnGenerated.Invoke();
    }

	public List<Vector2> LoadTerrainTypePoints (Texture2D source, float threshold = 0.5f)
	{

		List<Vector2> fgPoints = new List<Vector2>();
		List<Vector2> bgPoints = new List<Vector2>();
		Color color = new Color(0f,0f,0f,0f);
		Color red = Color.red;
		for (int x = 0; x < source.width; x++)
		{
			for (int y = 0; y < source.height; y++)
			{
				Color c = source.GetPixel(x,y);
				if (source.GetPixel(x,y) !=  red && (
					source.GetPixel(x-1,y) == red ||
					source.GetPixel(x+1,y )== red ||
					source.GetPixel(x,y+1) == red ||
					source.GetPixel(x,y-1) == red))
					{
						// Debug.Log(c);
						fgPoints.Add(new Vector2(x, y));
					}
			}
		}

    	return fgPoints;
	}

	public void FloodFill(Texture2D texture, Vector2 pos, Stack<Vector2> fgPoints, Color color)
	{
		// Stack<Vector2> fgPoints = new Stack<Vector2>();
		// fgPoints.Push(pos);
		// texture.SetPixel((int)pos.x, (int)pos.y, Color.cyan)3
		while(fgPoints.Count != 0)
		{
			Vector2 point = fgPoints.Pop();
			int x = (int)point.x;
			int x1 = x;
			int y = (int)point.y;

			Color c = texture.GetPixel(x,y);
			// texture.SetPixel(x,y, Color.cyan);
			while(x1 >= 0 && texture.GetPixel(x1,y) == color)
			{
				x1--;
			}
			x1++;
			bool spanAbove = false;
			bool spanBelow = false;
			Color red = Color.red;
			
			while(x1 < texture.width && texture.GetPixel(x1,y) == color)
			{
			 	texture.SetPixel(x1, y, Color.red);
			 	if(!spanAbove && y > 0 && texture.GetPixel(x1, y-1) == color)
			 	{
			 		fgPoints.Push(new Vector2(x1, y-1));
			 		spanAbove = true;
			 	}
				else if(spanAbove && y > 0 && texture.GetPixel(x1, y-1) != color)
				{
					spanAbove = false;
				}

				if(!spanBelow && y < texture.height -1 && texture.GetPixel(x1, y+1) == color)
				{
					fgPoints.Push(new Vector2(x1,y+1));
					spanBelow = true;
				}
				else if(spanBelow && y < texture.height -1 && texture.GetPixel(x1, y+1) != color)
				{
					spanBelow = false;
				}
				x1++;
			}
		}
	}




	private void InitSpriteDimensions() {
		widthWorld = sr.bounds.size.x;
		heightWorld = sr.bounds.size.y;
		widthPixel = sr.sprite.texture.width;
		heightPixel = sr.sprite.texture.height;
		
		Destroy(GetComponent<PolygonCollider2D>());
		gameObject.AddComponent<PolygonCollider2D>();
	}

	void Update () {

		Vector3 wPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D c = Physics2D.Raycast(wPos,wPos, 1f);
		if(c.collider != null)
		{
			if(Input.GetMouseButtonDown(0))
			{
				CircleCollider2D coll = new GameObject("cc", typeof(CircleCollider2D)).GetComponent<CircleCollider2D>();
				coll.radius = 0.1f;
				coll.transform.position = wPos;
                coll.isTrigger = true;
                DestroyGround(coll);
			}
			if(Input.GetMouseButtonDown(1))
			{
				Generate();
			}
		}
	}

	public void DestroyGround( CircleCollider2D cc ){
		Debug.Log ("chamou DestroyGround");

		V2int c = World2Pixel(cc.bounds.center.x, cc.bounds.center.y);
		// c => centro do circulo de destruiçao em pixels
		int r = Mathf.RoundToInt(cc.bounds.size.x*widthPixel/widthWorld);
		// r => raio do circulo de destruiçao em

		int x, y, px, nx, py, ny, d;

		for (x = 0; x <= r; x++)
		{
			d = (int)Mathf.RoundToInt(Mathf.Sqrt(r * r - x * x));

			for (y = 0; y <= d; y++)
			{
				px = c.x + x;
				nx = c.x - x;
				py = c.y + y;
				ny = c.y - y;

				sr.sprite.texture.SetPixel(px, py, transp);
				sr.sprite.texture.SetPixel(nx, py, transp);
				sr.sprite.texture.SetPixel(px, ny, transp);
				sr.sprite.texture.SetPixel(nx, ny, transp);
			}
		}

		sr.sprite.texture.Apply();

        Destroy(cc.gameObject);
        Destroy(GetComponent<PolygonCollider2D>());
		gameObject.AddComponent<PolygonCollider2D>();
	}



	private V2int World2Pixel(float x, float y) {
		V2int v = new V2int();

		float dx = x-transform.position.x;
		v.x = Mathf.RoundToInt(0.5f*widthPixel+ dx*widthPixel/widthWorld);

		float dy = y - transform.position.y;
		v.y = Mathf.RoundToInt(0.5f * heightPixel + dy * heightPixel / heightWorld);

		return v;
	}
}