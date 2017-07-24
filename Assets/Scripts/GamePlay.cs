using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour {

	public int typeHit;
	private GameObject clickedTank = null;
	private GameObject clickedCore = null;
	private string tankName = "";


	void Update () {
		if (Input.GetMouseButton(0)) {
			HandleInput();
		}
	}
	void CheckInput (RaycastHit hit) {
		if (hit.transform.tag == "Tank") {
			typeHit = 1;
			clickedTank = hit.collider.gameObject;
		}

		if (hit.transform.tag == "Core") {
			typeHit = 2;
			clickedCore = hit.collider.gameObject;
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			CheckInput (hit);
				
			switch (typeHit) {
			case 1:
				if (hit.transform.name == "Hex Mesh(Clone)") {
					clickedTank.GetComponent<TankScript> ().moveTo = hit.transform.gameObject;
				} else
					CheckInput(hit);
				break;
			case 2:
				if (hit.transform.name == "Hex Mesh(Clone)") {
					clickedCore.GetComponent<CoreScript> ().isSpawn = true;
					clickedCore.GetComponent<CoreScript> ().target = hit.transform.gameObject;
					typeHit = 0;
				} else
					CheckInput(hit);
				break;
			}
		}
	}
	void TouchCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);
	}
}