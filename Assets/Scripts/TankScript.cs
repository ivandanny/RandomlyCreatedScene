using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour {

	public HexGrid hexGrid;
	public int randomStart;

	void Start () {
		Debug.Log (hexGrid.selectedIslands[5].YPos);
		randomStart = Random.Range (0, hexGrid.selectedIslands.Count);
		int x = hexGrid.selectedIslands [randomStart].YPos;
		int y = hexGrid.selectedIslands [randomStart].XPos;
		transform.position = new Vector3 ((x + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f),5.0f,(y*(HexMetrics.outerRadius*1.5f)));

	}

	// Update is called once per frame
	void Update () {
		//transform.position = Vector3.Lerp(this.transform.position, new Vector3(5,5,5), Time.time/500);
	}

	void MovetoPoint(float x, float y, float z, float duration) {
		
	}
		
	void OnCollisionEnter (Collision collision) {
		Debug.Log ("Collide!");
		if (collision.gameObject.transform.tag == "Tank") {
			Debug.Log ("Tank!!!");
		}
	}
}
