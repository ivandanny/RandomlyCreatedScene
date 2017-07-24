using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreScript : MonoBehaviour {

	public TankScript tankScript;
	
	public HexGrid hexGrid;
	private int randomStart;
	public int playerNo;
	public bool isSpawn = false;
	public GameObject target = null;

	// Use this for initialization
	void Start () {
		randomStart = Random.Range (0, hexGrid.selectedIslands.Count	);
		int x = hexGrid.selectedIslands [randomStart].YPos;
		int y = hexGrid.selectedIslands [randomStart].XPos;
		Debug.Log (x +"    " + y);
		transform.position = new Vector3 ((x + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f),6.0f,(y*(HexMetrics.outerRadius*1.5f)));

	}

	void SpawnTank (int player, GameObject moveTo) {
		TankScript tank = Instantiate <TankScript> (tankScript);
		tank.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;
		//tank.transform.SetParent (transform, true);
		Vector3 position = moveTo.transform.position;
		position.y = 5.0f;
		tank.transform.localPosition = position;
		isSpawn = false;
		target = null;
	}

	// Update is called once per frame
	void Update () {
		if (isSpawn = true && target != null) {
			SpawnTank (playerNo, target);
		}
	}
}
