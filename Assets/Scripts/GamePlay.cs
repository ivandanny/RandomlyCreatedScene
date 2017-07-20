using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour {

	public int tankHit;
	private bool isMove = false;
	private GameObject clickedTank = null;
	private GameObject clickedCell = null;
	private string tankName = "";

	void Update () {
		if (Input.GetMouseButton(0)) {
			HandleInput();
		}
		if (isMove = true && clickedTank != null && clickedCell != null) {
			clickedTank.transform.position = Vector3.Lerp(clickedTank.transform.position, 
														new Vector3 (clickedCell.transform.position.x, 5.0f, clickedCell.transform.position.z)
														, .1f);
			if ((Mathf.Abs(clickedCell.transform.position.x - clickedTank.transform.position.x) < .3f) && 
				(Mathf.Abs(clickedTank.transform.position.z - clickedCell.transform.position.z)) < .3f) {
				isMove = false;
			}
		}

	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			if (hit.transform.tag == "Tank") {
				tankHit = 1;
				isMove = false;
				if (tankName != hit.transform.name) {
					clickedCell = null;
					tankName = hit.transform.name;
				}
				clickedTank = hit.collider.gameObject;
			} else {
				if (hit.transform.name == "Hex Mesh(Clone)" && tankHit == 1) {
					clickedCell = hit.collider.gameObject;
					isMove = true;
				}
			}
		}
	}

	void TouchCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		//Debug.Log("touched at " + position);
	}
}
