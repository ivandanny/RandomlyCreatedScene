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

	public GameObject dice1;
	public GameObject dice2;
	public GameObject tank1;
	public GameObject tank2;
	public bool fight = false;
	private int diceClick = 0;

	void Update () {
		if (Input.GetMouseButton(0) && fight == false) {
			HandleInput();
		}
		if (fight == true) {
			dice1.transform.parent.gameObject.SetActive (true);
			dice1.gameObject.SetActive (true);
			dice2.transform.parent.gameObject.SetActive (true);
			dice2.gameObject.SetActive (true);
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
		

	// Combat Code

	public void CombatPhase(GameObject dice){
		if (dice.GetComponent<DiceRoll> ().isClicked == false) {
			dice.GetComponent<DiceRoll> ().isClicked = true;
			diceClick += 1;
			if (diceClick == 2) {
				int dicevalue1 = int.Parse (dice1.GetComponent<DiceRoll> ().diceValue.text);
				int dicevalue2 = int.Parse (dice2.GetComponent<DiceRoll> ().diceValue.text);

				//Destroying tanks
				if (dicevalue1 > dicevalue2) {
					tank2.GetComponent<TankScript> ().killed ();
				} else {
					if (dicevalue2 > dicevalue1) { 
						tank1.GetComponent<TankScript> ().killed ();
					} else {
						tank1.GetComponent<TankScript> ().killed ();
						tank2.GetComponent<TankScript> ().killed ();
					}
				}
				fight = false;
				dice1.GetComponent <DiceRoll> ().isClicked = false;
				dice2.GetComponent <DiceRoll> ().isClicked = false;

				//Should add some delay
				dice1.transform.parent.gameObject.SetActive (false);
				dice1.gameObject.SetActive (false);
				dice2.transform.parent.gameObject.SetActive (false);
				dice2.gameObject.SetActive (false);
			}
		}
	}

	public void AttackP1() {
		CombatPhase (dice1);
	}

	public void AttackP2() {
		CombatPhase (dice2);
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