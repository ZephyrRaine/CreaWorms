using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMaker : MonoBehaviour 
{
    public Material meshMat;
    public void Generate (GameObject go) 
	{
		for(int i = transform.childCount -1; i>=0; i--)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
         PolygonCollider2D pc2 = go.GetComponent<PolygonCollider2D>();

		 for(int i = 0; i < pc2.pathCount; i++)
		 {
			Vector2[] points = pc2.GetPath(i);
			int pointCount = points.Length;
			Vector3[] vertices = new Vector3[pointCount];
			Vector2[] uv = new Vector2[pointCount];

			ColliderToMesh ctm = new GameObject("ctm"+i,typeof(ColliderToMesh)).GetComponent<ColliderToMesh>();
			ctm.transform.SetParent(transform, false);
			for(int j=0; j<pointCount; j++){
				Vector2 actual = points[j];
				vertices[j] = new Vector3(actual.x, actual.y, 0);
				uv[j] = actual;
			}
			ctm.InitMesh(points, vertices, uv, meshMat);
		 }
    
         //Render thing
     }
}
