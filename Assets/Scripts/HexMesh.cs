﻿using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {
	
	Mesh hexMesh;
	List<Vector3> vertices;
	List<int> triangles;

	MeshCollider meshCollider;
	
	public void Triangulate () {
		hexMesh.Clear();
		vertices.Clear();
		triangles.Clear();
		Vector3 center = new Vector3 (0, 0, 0);
		for (int i = 0; i < 6; i++) {
			AddTriangle (
				center,
				center + HexMetrics.corners [i],
				center + HexMetrics.corners [i + 1]
				);
		}
		/*for (int i = 0; i < cells.Length; i++) {
			Triangulate(cells[i]);
		}*/
		hexMesh.vertices = vertices.ToArray();
		hexMesh.triangles = triangles.ToArray();
		hexMesh.RecalculateNormals();

		meshCollider.sharedMesh = hexMesh;
	}

	
	void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}

	
	void Awake () {
		GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
		meshCollider = gameObject.AddComponent<MeshCollider>();
		hexMesh.name = "Hex Mesh";
		vertices = new List<Vector3>();
		triangles = new List<int>();
	}

	void Start () {
		Triangulate ();
	}
}