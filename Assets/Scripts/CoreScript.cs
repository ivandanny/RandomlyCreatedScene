using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreScript : MonoBehaviour {
	
	public HexGrid hexGrid;
	private int randomStart;

	// Use this for initialization
	void Start () {
		randomStart = Random.Range (0, hexGrid.selectedIslands.Count);
		int x = hexGrid.selectedIslands [randomStart].YPos;
		int y = hexGrid.selectedIslands [randomStart].XPos;
		Debug.Log (x +"    " + y);
		transform.position = new Vector3 ((x + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f),6.0f,(y*(HexMetrics.outerRadius*1.5f)));

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
