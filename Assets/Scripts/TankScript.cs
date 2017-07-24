using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour {

	public HexGrid hexGrid;
	private int randomStart;
	public GameObject moveTo = null;
	public int playerNo;

	public void TankMoveTo(GameObject target) {
		transform.position = Vector3.Lerp (transform.position, 
			new Vector3 (target.transform.position.x, 5.0f, target.transform.position.z), .005f);
		if ((Mathf.Abs (this.transform.position.x - target.transform.position.x) < .003f) &&
		    (Mathf.Abs (this.transform.position.z - target.transform.position.z)) < .003f) {
			target = null;
		}
	}

	// Update is called once per frame
	void Update () {
		if (moveTo != null) {
			TankMoveTo (moveTo);
		}
	}
}
