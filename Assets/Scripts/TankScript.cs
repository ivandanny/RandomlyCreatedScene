using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour {

	public HexGrid hexGrid;
	private int randomStart;
	public GameObject moveTo = null;
	public int playerNo;
	public bool onCombat = false;
	public int attack;
	public GameObject enemy = null;

	public HitScript hitScript;
	public float speed;

	public void TankMoveTo(GameObject target) {
		//transform.position = Vector3.Lerp (transform.position, new Vector3 (target.transform.position.x, 5.0f, target.transform.position.z), Time.deltaTime);
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, new Vector3 (target.transform.position.x, 5.0f, target.transform.position.z), step);
		if ((Mathf.Abs (transform.position.x - target.transform.position.x) < .003f) && (Mathf.Abs (transform.position.z - target.transform.position.z)) < .003f) {
			target = null;
		}
	}


	void Start () {
		attack = Random.Range (1, 6);
	}
	// Update is called once per frame
	void Update () {
		if (moveTo != null && onCombat == false) {
			TankMoveTo (moveTo);
		}


		if (onCombat == true && enemy != null) {
			//Debug.Log (playerNo + " Player damage: " + attack + "   compared to   " + enemy.GetComponent<TankScript> ().attack);
			if (enemy.GetComponent<TankScript> ().attack > attack) {
				enemy.GetComponent<TankScript> ().attack = Random.Range (1, 7);
				Destroy (this.gameObject);
			} else {
				if (enemy.GetComponent<TankScript> ().attack == attack) {
					Destroy (enemy.gameObject);
					Destroy (this.gameObject);
				} else {
					attack = Random.Range (1, 7);
					Destroy (enemy.gameObject);
					enemy = null;
					onCombat = false;
				}	
			}
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject != null) {
			if (collision.gameObject.tag == "Tank" && collision.gameObject.GetComponent<TankScript> ().playerNo < playerNo) {
				HitScript particle = Instantiate <HitScript> (hitScript);
				particle.transform.position = transform.position;
				enemy = collision.gameObject;
				onCombat = true;
			}
		}
	}
}
