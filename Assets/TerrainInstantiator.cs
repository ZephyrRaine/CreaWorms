using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainInstantiator : MonoBehaviour {
    public GameObject prefab;

	private void Start()
	{
		
        for (int i = 0; i < 1; i++)
		{
            RequestTerrain();
        }
        StartCoroutine(InstantiateRequested());
    }
    public IEnumerator AddTerrain()
	{
        WormsTerrain t = Instantiate(prefab, transform.position + Vector3.zero + Vector3.up * 10.24f * transform.childCount, Quaternion.identity, transform).GetComponent<WormsTerrain>();
        t.OnGenerated.AddListener(Finished);
        yield return t.Generate();
    }
    WormController[] wc;
    public void Finished()
	{
		requestCount--;
        Debug.Log(requestCount);
        if(requestCount == 0)
		{
            wc = FindObjectsOfType<WormController>();
            wc[0].Activate(true);
            DestroyTerrain.Instantiate(wc[0].transform.position, 0.15f);
        }
    }
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.T))
		{
            wc[0].Activate(false);
            wc[1].Activate(true);
            DestroyTerrain.Instantiate(wc[1].transform.position, 0.15f);
        }
	}
    int requestCount;
    internal void RequestTerrain()
    {
        requestCount++;
    }

	public IEnumerator InstantiateRequested()
	{
        for (int i = 0; i < requestCount; i++)
		{
            yield return AddTerrain();
        }
    }
}
