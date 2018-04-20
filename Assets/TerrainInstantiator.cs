using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainInstantiator : MonoBehaviour {
    public GameObject prefab;
    public TMPro.TMP_Text tt;
    public GameObject mer;
	private void Start()
	{
        StartCoroutine(firsttime());
    }

    public IEnumerator firsttime()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return InstantiateRequested();
        }
        Finished();
    }
    bool start = true;
    public IEnumerator AddTerrain()
	{
        WormsTerrain t = Instantiate(prefab, transform.position + Vector3.zero + Vector3.up * 10.24f * transform.childCount, Quaternion.identity, transform).GetComponent<WormsTerrain>();
        if(start)
        {
            // t.OnGenerated.AddListener(Finished);
        }
        yield return t.Generate();
    }
    WormController[] wc;
    int currentActive;
    public void Finished()
	{
        Debug.Log("OKOK");
        if(start)
		{
            tt.text = "";
            wc = FindObjectsOfType<WormController>();
            currentActive = 0;
            start = false;
            waitForInput = true;
        }
    }
	private void Update()
	{
		if(waitForInput)
        {
            tt.text = "Player " + currentActive + " press mouse";
            if(Input.GetMouseButtonDown(0))
            {
                tt.text = "";
                waitForInput = false;
                FinishSwitch();
            }
        }


    }

    public void Switch()
    {
        wc[currentActive].Activate(false);
        Debug.Log(currentActive);
        
        currentActive = currentActive ^ 1;
        Invoke("Activate", 0.5f);
        if(currentActive == 0)
            mer.transform.position += Vector3.up*10f;
    }
    bool waitForInput = false;
    void Activate()
    {
        waitForInput = true;
    }

    void FinishSwitch()
    {
        Debug.Log(currentActive);
        wc[currentActive].Activate(true);
        Invoke("Switch", 7f);
    }
    int requestCount;
    internal void RequestTerrain()
    {
        StartCoroutine(InstantiateRequested());
    }

	public IEnumerator InstantiateRequested()
	{
        yield return AddTerrain();
    }
}
