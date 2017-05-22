﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour {

	public HexCell cellPrefab;
	HexCell[] cells;
	private string result;
	public HexMesh hexMesh;

	//Grid Function Variable
	public static int gridColumn = 15;
	public static int gridRow = 15;
	private int initLand = 0;

	//Perlin Funcion Variable
	public int[,] gridList = new int[gridRow,gridColumn];
	public float initStart = 0.55f;
	public float zoom = 0.3f;
	public float shiftIndicator = 0.3f;

	//Variable for Checking Island
	List<int[,,]> islands; //x pos, y pos, and counter no
	int landCounter = 1;
	int[,] landList = new int[gridRow, gridColumn];

	//Label Text on Hexagon
	public Text cellLabelPrefab;
	Canvas gridCanvas;

	void generateLand(int heightLoc, int widthLoc) {
		gridList[heightLoc, widthLoc] = 1; 
	}
	
	//to declare an Island
	void hexFind (int row, int column,int landNo) {
		int[] search = new int[] {1,1,1,1,1,1};
		int counter = 0;

		if (row == 0) {
			search [2] = 0;
			search [3] = 0;
		}
		if (column ==0) {
			if (row%2 == 0 ) {
				search [3] = 0;
				search [4] = 0;
				search [5] = 0;
			} else {
				search [4] = 0;
			}
		}
		if (row == gridRow-1) {
			search [0] = 0;
			search [5] = 0;
		}
		if (column == gridColumn-1) {
			if (row%2 == 0) {
				search [1] = 0;
			} else {
				search [0] = 0;
				search [1] = 0;
				search [2] = 0;
			}
		}
		Debug.Log (row.ToString () + ", " + column.ToString ());

		for (int i = 0; i < 6; i++) {
			if (row %2 == 0 && search[i] == 1) {
				switch (i) {
				case 0:
					if (landList[row+1,column] > 0 ) {
						Debug.Log ("Check for:" + i + " Value = " + landList[row+1,column]);
						landList[row,column] = landList[row+1,column];
						Debug.Log ("Now = " + landList[row,column]);
						counter++;
					}
					break;
				case 1:
					if (landList[row,column+1] > 0 ) {
						Debug.Log ("Check for:" + i + " Value = " + landList[row,column+1]);
						landList[row,column] = landList[row,column+1];
						Debug.Log ("Now = " + landList[row,column]);
						counter++;
					}
					break;
				case 2:
					if (landList[row-1,column] > 0 ) {
						Debug.Log ("Check for:" + i + " Value = " + landList[row-1,column]);
						landList[row,column] = landList[row-1,column];
						Debug.Log ("Now = " + landList[row,column]);
						counter++;
					}
					break;
				case 3:
					if (landList[row-1,column-1] > 0 ) {
						Debug.Log ("Check for:" + i + " Value = " + landList[row-1,column-1]);
						landList[row,column] = landList[row-1,column-1];
						Debug.Log ("Now = " + landList[row,column]);
						counter++;
					}
					break;
				case 4:
					if (landList[row,column-1] > 0 ) {
						Debug.Log ("Check for:" + i + " Value = " + landList[row,column-1]);
						landList[row,column] = landList[row,column-1];
						Debug.Log ("Now = " + landList[row,column]);
						counter++;
					}
					break;
				case 5:
					if (landList[row+1,column-1] > 0 ) {
						Debug.Log ("Check for:" + i + " Value = " + landList[row+1,column-1]);
						landList[row,column] = landList[row+1,column-1];
						Debug.Log ("Now = " + landList[row,column]);
						counter++;
					}
					break;
				}
			}
			if (row %2 == 1 && search[i] == 1) {
				switch (i) {
				case 0:
					if (landList[row+1,column+1] > 0 ) {
						Debug.Log ("Check for:" + i + " Value = " + landList[row+1,column+1]);
						landList[row,column] = landList[row+1,column+1];
						Debug.Log ("Now = " + landList[row,column]);
						counter++;
					}
					break;
				case 1:
					if (landList[row,column+1] > 0 ) {
						Debug.Log ("Check for:" + i + " Value = " + landList[row,column+1]);
						landList[row,column] = landList[row,column+1];
						Debug.Log ("Now = " + landList[row,column]);
						counter++;
					}
					break;
				case 2:
					if (landList[row-1,column+1] > 0 ) {
						Debug.Log ("Check for:" + i + " Value = " + landList[row-1,column+1]);
						landList[row,column] = landList[row-1,column+1];
						Debug.Log ("Now = " + landList[row,column]);
						counter++;
					}
					break;
				case 3:
					if (landList[row-1,column] > 0 ) {
						Debug.Log ("Check for:" + i + " Value = " + landList[row-1,column]);
						landList[row,column] = landList[row-1,column];
						Debug.Log ("Now = " + landList[row,column]);
						counter++;
					}
					break;
				case 4:
					if (landList[row,column-1] > 0 ) {
						Debug.Log ("Check for:" + i + " Value = " + landList[row,column-1]);
						landList[row,column] = landList[row,column-1];
						Debug.Log ("Now = " + landList[row,column]);
						counter++;
					}
					break;
				case 5:
					if (landList[row+1,column] > 0 ) {
						Debug.Log ("Check for:" + i + " Value = " + landList[row+1,column]);
						landList[row,column] = landList[row+1,column];
						Debug.Log ("Now = " + landList[row,column]);
						counter++;
					}
					break;
				}
			}
		}
		Debug.Log ("LandCounter= " + landCounter + "   Counter= " + counter);
		if (counter < 1){
			landList [row, column] = landCounter;
			Debug.Log ("Now = " + landList[row,column]);
			//string res = search[0].ToString() + search[1].ToString() + search[2].ToString() + search[3].ToString() + search[4].ToString + search[5].ToString();
			//Debug.Log (search[0]);
			//Debug.Log (search[1]);
			//Debug.Log (search[2]);
			//Debug.Log (search[3]);
			//Debug.Log (search[4]);
			//Debug.Log (search[5]);
			landCounter++;
		}
		Debug.Log (" ");
//		string around = "";
//		around = (search [0].ToString() + " " + search [1].ToString() + " " + search [2].ToString() + " " + search [3].ToString() +
//			" " + search [4].ToString() + " " + search [5].ToString());
//		Debug.Log (around);
	}
	

	// Use this for initialization
	void initRandom (int row, int column) {

		Vector2 shift = new Vector2(shiftIndicator,shiftIndicator); // play with this to shift map around
		int offset = Random.Range (0, 1000);
		for(int x = offset; x < (row+offset); x++)
			for(int y = offset; y < (column+offset); y++)
		{
			Vector2 pos = zoom * (new Vector2(x,y)) + shift;
			Vector2 pos2 = pos*4;
			float noise = Mathf.PerlinNoise(pos.x, pos.y);
			noise += (Mathf.PerlinNoise ((pos2.x), (pos2.y))-0.5f)/2.5f;
			if (noise<(1-initStart)) gridList[(x-offset),(y-offset)] = 0;
			else {
				generateLand ((x-offset),(y-offset));
			}
		}
		/*for (int i=0; i< initLand; i++) {
			int x = Random.Range (0,gridRow);
			int y = Random.Range (0,gridColumn);
			if (gridList[x,y] == 1){
				i -= 1;
			}
			generateLand(x,y);
		} */
	}

	void Awake () {
		initLand = Mathf.RoundToInt(gridRow * gridColumn * initStart);
		initRandom(gridRow,gridColumn);

		//Checking the Island
		for (int i = 0; i < gridRow; i++) {
			for (int j = 0; j < gridColumn; j++){
				if (gridList[i,j] > 0) {
					hexFind(i,j,landCounter);
				}
			}
		}

		cells = new HexCell[gridRow * gridColumn];

		gridCanvas = GetComponentInChildren<Canvas>();
		
		for (int z = 0, i = 0; z < gridRow; z++) {
			for (int x = 0; x < gridColumn; x++) {
				if (gridList[z,x] >= 1) {
					CreateCell(x, z, i++);
				}
			}
		}
	}
	

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);
		
		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;

		HexMesh hex = Instantiate<HexMesh>(hexMesh);
		hex.transform.SetParent(transform, false);
		hex.transform.localPosition = position;

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
		//label.text = landList[x,z].ToString();
		label.text = z.ToString() + "\n" + x.ToString() + "\n" + landList[z,x].ToString();
	}

	
}