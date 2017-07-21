using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour {

	public int typeHit;
	public bool isMove = false;
	private GameObject clickedTank = null;
	private GameObject clickedCell = null;
	private string tankName = "";

	public TankScript tankScript;

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
				isMove = false;
				clickedCell = null;
				typeHit = 1;
				clickedTank = hit.collider.gameObject;
			}

			if (hit.transform.tag == "Core") {
				clickedTank = null;
				clickedCell = null;
				isMove = false;
				typeHit = 2;
			}
				
			switch (typeHit) {
			case 1:
				if (hit.transform.name == "Hex Mesh(Clone)") {
					clickedCell = hit.collider.gameObject;
					tankScript.moveTo = clickedCell;
					isMove = true;
				}

				if (hit.transform.tag == "Tank") {
					clickedCell = hit.collider.gameObject;
					typeHit = 1;
					isMove = false;
				}

				if (hit.transform.tag == "Core") {
					clickedCell = hit.collider.gameObject;
					typeHit = 2;
					isMove = false;
				}
				break;
			case 2:
				isMove = false;
				if (hit.transform.name == "Hex Mesh(Clone)") {
					isMove = false;
					clickedTank = null;
					clickedCell = hit.collider.gameObject;
					TankScript tank = Instantiate <TankScript> (tankScript);
					tank.transform.SetParent(transform, false);
					Vector3 position = clickedCell.transform.position;
					position.y = 5.0f;
					tank.transform.position = position;
					typeHit = 0;
				}
				break;
			}
		}

	void TouchCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		//Debug.Log("touched at " + position);
	}
}