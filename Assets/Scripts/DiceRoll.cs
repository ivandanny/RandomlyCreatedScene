using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour {
	public Text diceValue;
	public bool isClicked = false;

	// Use this for initialization
	void Start () {
		diceValue = GetComponent<Text> ();
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (isClicked == false) {
			diceValue.text = Random.Range (1, 7).ToString ();
		}
	}
}
