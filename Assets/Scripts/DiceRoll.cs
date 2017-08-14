using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour {
	Text diceValue;

	// Use this for initialization
	void Start () {
		diceValue = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		diceValue.text = Random.Range (1, 7).ToString();
	}
}
