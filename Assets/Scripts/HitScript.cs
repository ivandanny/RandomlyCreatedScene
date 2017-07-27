using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (OnDestroy ());
	}

	IEnumerator OnDestroy () {
		yield return new WaitForSeconds (0.6f);
		Destroy (this.gameObject);
	}
}
