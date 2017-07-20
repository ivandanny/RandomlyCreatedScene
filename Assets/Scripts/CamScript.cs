using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour {

	private CamScript camScript;

	private int highest;
	//private 

	// Use this for initialization
	void Awake () {
		if (HexGrid.gridRow < HexGrid.gridColumn) {
			highest = HexGrid.gridColumn;
		} else {
			highest = HexGrid.gridRow;
		}
		transform.position = new Vector3 ((highest * 8.2f), (highest * 12.2f), 0);
		Camera.main.orthographicSize = highest * 7.3f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
