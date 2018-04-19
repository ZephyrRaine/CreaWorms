using UnityEngine;
using System.Collections.Generic;

 [RequireComponent(typeof(MeshFilter))]
 [RequireComponent(typeof(MeshRenderer))]
 public class ColliderToMesh : MonoBehaviour {
	 
     public void InitMesh(Vector2[] points, Vector3[] vertices, Vector2[] uv) 
	 {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        Triangulator tr = new Triangulator(points);
        int [] triangles = tr.Triangulate();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        List<Vector3> normals = new List<Vector3>();
        foreach(Vector2 point in vertices)
        {
            normals.Add(Vector3.forward);
        }
        mesh.SetNormals(normals);
        mf.mesh = mesh;
     }
 }