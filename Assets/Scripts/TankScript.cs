using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
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
