	using UnityEngine;
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

	// Connecting the Island
	int[,] conList = new int[gridRow,gridColumn];
	int radius = 0;

	//Perlin Funcion Variable
	public int[,] gridList = new int[gridRow,gridColumn];
	public float initStart = 0.55f;
	public float zoom = 0.3f;
	public float shiftIndicator = 0.3f;

	//Variable for Checking Island
	List<int[,,]> islands; //x pos, y pos, and counter no
	int landCounter = 0;
	int[,] landList = new int[gridRow, gridColumn];

	//Label Text on Hexagon
	public Text cellLabelPrefab;
	Canvas gridCanvas;
	

	//Check to prevent array out of Index
	int[] indexCheck (int x, int y) {
		int[] value = {1,1,1,1,1,1};
		if (x == 0) {
			value [2] = 0;
			value [3] = 0;
		}
		if (y ==0) {
			if (x%2 == 0 ) {
				value [3] = 0;
				value [4] = 0;
				value [5] = 0;
			} else {
				value [4] = 0;
			}
		}
		if (x == gridRow-1) {
			value [0] = 0;
			value [5] = 0;
		}
		if (y == gridColumn-1) {
			if (x%2 == 0) {
				value [1] = 0;
			} else {
				value [0] = 0;
				value [1] = 0;
				value [2] = 0;
			}
		}
		return value;
	}
	
	//to declare an Island
	void islandTag (int row, int column, int counter) {

		initLand = Mathf.RoundToInt (gridRow * gridColumn * initStart);
		landList [row, column] = counter;
		int[] search = indexCheck (row, column);
		for (int i = 0; i < 6; i++) {
			if (row %2 == 0 && search[i] == 1) {
				switch (i) {
				case 0:
					if (landList[row+1,column] != counter && gridList[row+1,column] == 1 ) {
						islandTag(row+1,column,counter);
					}
					break;
				case 1:
					if (landList[row,column+1] != counter && gridList[row,column+1] == 1 ) {
						islandTag(row,column+1,counter);
					}
					break;
				case 2:
					if (landList[row-1,column] != counter && gridList[row-1,column] == 1 ) {
						islandTag(row-1,column,counter);
					}
					break;
				case 3:
					if (landList[row-1,column-1] != counter && gridList[row-1,column-1] == 1 ) {
						islandTag(row-1,column-1,counter);
					}
					break;
				case 4:
					if (landList[row,column-1] != counter && gridList[row,column-1] == 1 ) {
						islandTag(row,column-1,counter);
					}
					break;
				case 5:
					if (landList[row+1,column-1] != counter && gridList[row+1,column-1] == 1 ) {
						islandTag(row+1,column-1,counter);
					}
					break;
				}
			}
			if (row %2 == 1 && search[i] == 1) {
				switch (i) {
				case 0:
					if (landList[row+1,column+1] != counter && gridList[row+1,column+1] == 1 ) {
						islandTag(row+1,column+1,counter);
					}
					break;
				case 1:
					if (landList[row,column+1] != counter && gridList[row,column+1] == 1 ) {
						islandTag(row,column+1,counter);
					}
					break;
				case 2:
					if (landList[row-1,column+1] != counter && gridList[row-1,column+1] == 1 ) {
						islandTag(row-1,column,counter);
					}
					break;
				case 3:
					if (landList[row-1,column] != counter && gridList[row-1,column] == 1 ) {
						islandTag(row-1,column,counter);
					}
					break;
				case 4:
					if (landList[row,column-1] != counter && gridList[row,column-1] == 1 ) {
						islandTag(row,column-1,counter);
					}
					break;
				case 5:
					if (landList[row+1,column] != counter && gridList[row+1,column] == 1 ) {
						islandTag(row+1,column,counter);
					}
					break;
				}
			}
		}
	}
	

	// Using Perlin function, randomly allocate the Islands
	void initRandom (int row, int column) {

		Vector2 shift = new Vector2(shiftIndicator,shiftIndicator); // play with this to shift map around
		int offset = Random.Range (0, 10000);
		for(int x = offset; x < (row+offset); x++)
			for(int y = offset; y < (column+offset); y++)
		{
			Vector2 pos = zoom * (new Vector2(x,y)) + shift;
			Vector2 pos2 = pos*4;
			float noise = Mathf.PerlinNoise(pos.x, pos.y);
			noise += (Mathf.PerlinNoise ((pos2.x), (pos2.y))-0.5f)/2.5f;
			if (noise<(1-initStart)) gridList[(x-offset),(y-offset)] = 0;
			else {
				gridList[x-offset, y-offset] = 1; 
			}
		}
	}

	//iteration to find the connecting island
	void findCon ( int row, int column, int depth) {
		int[] search = indexCheck (row, column);
		foreach (int i in search) {
			if (i != 0) {
				if (row %2 == 0 && search[i] == 1) {
					switch (i) {
					case 0:
						if (landList [row + 1, column] == 1 && depth == 1) {
							gridList [row, column] = 1;
							return;
						} else {
							if (landList [row + 1, column] > 1) {
								islandTag (row, column, 1);
								landCounter -= 1;
								return;
							} else {
								if (conList [row + 1, column] == (depth - 1)) {
									conList [row, column] = depth;
								}
							}
						}
						break;
					case 1:
						if (landList [row, column+1] == 1 && depth == 1) {
							gridList [row, column] = 1;
							return;
						} else {
							if (landList [row, column + 1] > 1) {
								islandTag (row, column, 1);
								landCounter -= 1;
								return;
							} else {
								if (conList [row, column + 1] == (depth - 1)) {
									conList [row, column] = depth;
								}
							}
						}
						break;
					case 2:
						if (landList [row - 1, column] == 1 && depth == 1) {
							gridList [row, column] = 1;
							return;
						} else {
							if (landList [row - 1, column] > 1) {
								islandTag (row, column, 1);
								landCounter -= 1;
								return;
							} else {
								if (conList [row - 1, column] == (depth - 1)) {
									conList [row, column] = depth;
								}
							}
						}
						break;
					case 3:
						if (landList [row - 1, column-1] == 1 && depth == 1) {
							gridList [row, column] = 1;
							return;
						} else {
							if (landList [row - 1, column-1] > 1) {
								islandTag (row, column, 1);
								landCounter -= 1;
								return;
							} else {
								if (conList [row - 1, column-1] == (depth - 1)) {
									conList [row, column] = depth;
								}
							}
						}
						break;
					case 4:
						if (landList [row, column-1] == 1 && depth == 1) {
							gridList [row, column] = 1;
							return;
						} else {
							if (landList [row, column - 1] > 1) {
								islandTag (row, column, 1);
								landCounter -= 1;
								return;
							} else {
								if (conList [row, column - 1] == (depth - 1)) {
									conList [row, column] = depth;
								}
							}
						}
						break;
					case 5:
						if (landList [row+1, column-1] == 1 && depth == 1) {
							gridList [row, column] = 1;
							return;
						} else {
							if (landList [row+1, column - 1] > 1) {
								islandTag (row, column, 1);
								landCounter -= 1;
								return;
							} else {
								if (conList [row+1, column - 1] == (depth - 1)) {
									conList [row, column] = depth;
								}
							}
						}
						break;
					}
				}
				if (row %2 == 1 && search[i] == 1) {
					switch (i) {
					case 0:
						if (landList [row + 1, column + 1] == 1 && depth == 1) {
							gridList [row, column] = 1;
							return;
						} else {
							if (landList [row + 1, column + 1] > 1) {
								islandTag (row, column, 1);
								landCounter -= 1;
								return;
							} else {
								if (conList [row + 1, column + 1] == (depth - 1)) {
									conList [row, column] = depth;
								}
							}
						}
						break;
					case 1:
						if (landList [row, column+1] == 1 && depth == 1) {
							gridList [row, column] = 1;
							return;
						} else {
							if (landList [row, column + 1] > 1) {
								islandTag (row, column, 1);
								landCounter -= 1;
								return;
							} else {
								if (conList [row, column + 1] == (depth - 1)) {
									conList [row, column] = depth;
								}
							}
						}
						break;
					case 2:
						if (landList [row - 1, column+1] == 1 && depth == 1) {
							gridList [row, column] = 1;
							return;
						} else {
							if (landList [row - 1, column+1] > 1) {
								islandTag (row, column, 1);
								landCounter -= 1;
								return;
							} else {
								if (conList [row - 1, column+1] == (depth - 1)) {
									conList [row, column] = depth;
								}
							}
						}
						break;
					case 3:
						if (landList [row - 1, column] == 1 && depth == 1) {
							gridList [row, column] = 1;
							return;
						} else {
							if (landList [row - 1, column] > 1) {
								islandTag (row, column, 1);
								landCounter -= 1;
								return;
							} else {
								if (conList [row - 1, column] == (depth - 1)) {
									conList [row, column] = depth;
								}
							}
						}
						break;
					case 4:
						if (landList [row, column-1] == 1 && depth == 1) {
							gridList [row, column] = 1;
							return;
						} else {
							if (landList [row, column - 1] > 1) {
								islandTag (row, column, 1);
								landCounter -= 1;
								return;
							} else {
								if (conList [row, column - 1] == (depth - 1)) {
									conList [row, column] = depth;
								}
							}
						}
						break;
					case 5:
						if (landList [row + 1, column] == 1 && depth == 1) {
							gridList [row, column] = 1;
							return;
						} else {
							if (landList [row + 1, column] > 1) {
								islandTag (row, column, 1);
								landCounter -= 1;
								return;
							} else {
								if (conList [row + 1, column] == (depth - 1)) {
									conList [row, column] = depth;
								}
							}
						}
						break;
					}
				}
			}
		}
	}

	void Awake () {
		bool checkCon = false;
		initRandom(gridRow,gridColumn);
		int[] firstPoint = { 0, 0 };
		//Checking the Island
		for (int i = 0; i < gridRow; i++) {
			for (int j = 0; j < gridColumn; j++){
				if (gridList[i,j] > 0 && landList[i,j] == 0) {
					landCounter++;
					if (landCounter == 1) {
						firstPoint [0] = i;
						firstPoint [1] = j;
					}
					islandTag(i,j,landCounter);
				}
			}
		}
		/*
		bool check = false;
		int run = 0;
		while (landCounter > 1 && run != 6) {
			radius += 1;
			for (int i = 0; i < gridRow; i++) {
				for (int j = 0; j < gridRow; j++) {
					if (gridList [i, j] == 0) {
						findCon (i, j, radius);
						if (gridList [i, j] == 1) {
							check = true;
							break;
						}
					}
				}
				if (check == true)
					break;
			}
			if (check == true) {
				radius = 0;
				check = false;
			}
			run += 1;
		}*/

		Debug.Log (landCounter);

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