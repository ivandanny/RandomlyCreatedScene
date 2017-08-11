using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {
	public int countryNo = 0;
	public int countryOwner = 0;

	
	Mesh hexMesh;
	List<Vector3> vertices;
	List<int> triangles;

	public GameObject particleGlow;
	private GameObject gameplayObj;

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
		gameplayObj = GameObject.FindGameObjectWithTag ("GameController");
		particleGlow.SetActive (false);
		Triangulate ();
	}

	void Update () {
		if (countryNo == gameplayObj.GetComponent<GamePlay> ().currentCountry) {
			particleGlow.SetActive (true);
		} else
			particleGlow.SetActive (false);
		if (this.GetComponent<Renderer> ().material != gameplayObj.GetComponent<GamePlay> ().countryMat [countryOwner] && countryOwner != 0) {
				this.GetComponent<Renderer> ().material = gameplayObj.GetComponent<GamePlay> ().countryMat [countryOwner-1];
		}
		if (countryOwner != gameplayObj.GetComponent<GamePlay> ().countrySide [countryNo]) {
			countryOwner = gameplayObj.GetComponent<GamePlay> ().countrySide [countryNo];
		}
	}
}