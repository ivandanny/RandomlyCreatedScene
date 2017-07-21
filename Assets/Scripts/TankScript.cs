using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour {

	public HexGrid hexGrid;
	private int randomStart;
	public GameObject moveTo = null;

	// Update is called once per frame
	void Update () {
		if (moveTo != null) {
			transform.position = Vector3.Lerp (transform.position, 
				new Vector3 (moveTo.transform.position.x, 5.0f, moveTo.transform.position.z), .1f);
			if ((Mathf.Abs(moveTo.transform.position.x - moveTo.transform.position.x) < .3f) && 
				(Mathf.Abs(moveTo.transform.position.z - moveTo.transform.position.z)) < .3f) {
				moveTo = null;
			}
		}
	}
}
