using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour {

	public int typeHit;
	public int currentCountry;
	private GameObject clickedTank = null;
	private GameObject clickedCore = null;
	private string tankName = "";

	public Material[] countryMat;

	public int[] countrySide = new int[1000];


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
			if (hit.transform.tag == "Cell") {
				currentCountry = hit.collider.gameObject.GetComponent<HexMesh> ().countryNo;
			}
			switch (typeHit) {
			case 1:
				if (hit.transform.tag == "Cell" && clickedTank != null) {
					clickedTank.GetComponent<TankScript> ().moveTo = hit.transform.gameObject;
				} else
					CheckInput(hit);
				break;
			case 2:
				if (hit.transform.tag == "Cell") {
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