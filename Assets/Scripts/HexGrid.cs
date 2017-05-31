using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour {

	public HexCell cellPrefab;
	HexCell[] cells;
	private string result;
	public HexMesh hexMesh;

	//Grid Function Variable
	public static int gridColumn = 30;
	public static int gridRow = 30;
	int[] landCount = new int[gridRow*gridColumn/3];

	//Perlin Funcion Variable
	public int[,] gridList = new int[gridRow,gridColumn];
	public float initStart = 0.55f;
	public float zoom = 0.3f;
	public float shiftIndicator = 0.3f;

	//Variable for Checking Island
	public class Islands {
		public int XPos;
		public int YPos;
		public int No;

		public Islands (int xPos, int yPos, int no) {
			XPos = xPos;
			YPos = yPos;
			No = no;
		}
	}
	public List<Islands> islands = new List <Islands> () ;

	public class SelectedIslands {
		public int XPos;
		public int YPos;
		public int No;

		public SelectedIslands (int xPos, int yPos, int no) {
			XPos = xPos;
			YPos = yPos;
			No = no;
		}
	}
	public List<SelectedIslands> selectedIslands = new List <SelectedIslands> () ;

	int landCounter = 0;
	int[,] landList = new int[gridRow, gridColumn];

	//Label Text on Hexagon
	public Text cellLabelPrefab;
	Canvas gridCanvas;

	bool quit = false;

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
	void islandTag (int row, int column, int counter, int selected) {
		if (selected == 1) {
			selectedIslands.Add (new SelectedIslands(row, column,1));
		}
		landList [row, column] = counter;
		landCount [counter-1]++;
		int[] search = indexCheck (row, column);
		for (int i = 0; i < 6; i++) {
			if (row %2 == 0 && search[i] == 1) {
				switch (i) {
				case 0:
					if (landList[row+1,column] != counter && gridList[row+1,column] == 1 ) {
						islandTag(row+1,column,counter,selected);
					}
					break;
				case 1:
					if (landList[row,column+1] != counter && gridList[row,column+1] == 1 ) {
						islandTag(row,column+1,counter,selected);
					}
					break;
				case 2:
					if (landList[row-1,column] != counter && gridList[row-1,column] == 1 ) {
						islandTag(row-1,column,counter,selected);
					}
					break;
				case 3:
					if (landList[row-1,column-1] != counter && gridList[row-1,column-1] == 1 ) {
						islandTag(row-1,column-1,counter,selected);
					}
					break;
				case 4:
					if (landList[row,column-1] != counter && gridList[row,column-1] == 1 ) {
						islandTag(row,column-1,counter,selected);
					}
					break;
				case 5:
					if (landList[row+1,column-1] != counter && gridList[row+1,column-1] == 1 ) {
						islandTag(row+1,column-1,counter,selected);
					}
					break;
				}
			}
			if (row %2 == 1 && search[i] == 1) {
				switch (i) {
				case 0:
					if (landList[row+1,column+1] != counter && gridList[row+1,column+1] == 1 ) {
						islandTag(row+1,column+1,counter,selected);
					}
					break;
				case 1:
					if (landList[row,column+1] != counter && gridList[row,column+1] == 1 ) {
						islandTag(row,column+1,counter,selected);
					}
					break;
				case 2:
					if (landList[row-1,column+1] != counter && gridList[row-1,column+1] == 1 ) {
						islandTag(row-1,column,counter,selected);
					}
					break;
				case 3:
					if (landList[row-1,column] != counter && gridList[row-1,column] == 1 ) {
						islandTag(row-1,column,counter,selected);
					}
					break;
				case 4:
					if (landList[row,column-1] != counter && gridList[row,column-1] == 1 ) {
						islandTag(row,column-1,counter,selected);
					}
					break;
				case 5:
					if (landList[row+1,column] != counter && gridList[row+1,column] == 1 ) {
						islandTag(row+1,column,counter,selected);
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
				islands.Add (new Islands(x-offset,y-offset,0));
			}
		}
	}

	int[] findMax ( int [] corner) {
		
		int minX = selectedIslands[0].XPos;
		int maxX = selectedIslands[0].XPos;
		int minY = selectedIslands[0].YPos;
		int maxY = selectedIslands[0].YPos;
		for(int cnt = 0; cnt < selectedIslands.Count; cnt++) {
			if (selectedIslands [cnt].XPos < minX)
				minX = selectedIslands [cnt].XPos;
			if (selectedIslands [cnt].XPos > maxX)
				maxX = selectedIslands [cnt].XPos;
			if (selectedIslands [cnt].YPos < minY)
				minY = selectedIslands [cnt].YPos;
			if (selectedIslands [cnt].YPos > maxY)
				maxY = selectedIslands [cnt].YPos;
		}

		corner [0] = minX;
		corner [1] = maxX;
		corner [2] = minY;
		corner [3] = maxY;
		Debug.Log ("FindMax: " + corner [0] + ", " + corner [1] + ", " + corner [2] + ", " + corner [3]);
		return (corner);
	}

	//Move the first island to connect with the other island
	void combineIslands () {
		bool canMove = false;
		int[] border = { 0, 0, 0, 0 };
		int x = selectedIslands [0].XPos;
		int y = selectedIslands [0].YPos;
		border = findMax (border);

		while (border [1] < (gridRow-1) || canMove == false) {
			canMove = true;
			//Go Right
			while (border [3] < (gridColumn - 2)) {
				Debug.Log ("Right + 1");
				for (int cnt = 0; cnt < selectedIslands.Count; cnt++) {
					selectedIslands [cnt].YPos += 1;
					x = selectedIslands [cnt].XPos;
					y = selectedIslands [cnt].YPos;
					//Debug.Log ("Right  : " + x + ", " + y);
					if (y + 1 < gridColumn) {
						if (gridList [x, y] == 1 && landList [x, y] != 1) {
							Debug.Log ("Found Right" + landList [x, y]);
							islandTag (x, y, 1, 1);
						}
					}
				}
				border = findMax (border);
				if (border[1] >= gridRow) {
					for (int cnt = 0; cnt < selectedIslands.Count; cnt++) {
						selectedIslands [cnt].YPos -= 1;
					}
					border [0]--;
					border [1]--;
				}
			}
			//Go Left
			while (border [2] > 0) {
				Debug.Log ("Left + 1");
				for (int cnt = 0; cnt < selectedIslands.Count; cnt++) {
					selectedIslands [cnt].YPos -= 1;
					x = selectedIslands [cnt].XPos;
					y = selectedIslands [cnt].YPos;
					//Debug.Log ("Left  : " + x + ", " + y);
					if (y > 0) {
						if (gridList [x, y] == 1 && landList [x, y] != 1) {
							Debug.Log ("Found Left" + landList [x, y]);
							islandTag (x, y, 1, 1);
						}
					}
				}
				border = findMax (border);
				if (border[2] < 0) {
					for (int cnt = 0; cnt < selectedIslands.Count; cnt++) {
						selectedIslands [cnt].YPos += 1;
					}
					border [2]++;
					border [3]++;
				}
			}

			//Go Up
			if (border [1] < gridRow - 2) {
				Debug.Log ("UP + 1");
				for (int cnt = 0; cnt < selectedIslands.Count; cnt++) {
					selectedIslands [cnt].XPos += 2;
					x = selectedIslands [cnt].XPos;
					y = selectedIslands [cnt].YPos;
					//Debug.Log ("UP  : " + x + ", " + y);
					if (x + 2 < gridRow) {
						if (gridList [x, y] == 1 && landList [x, y] != 1) {
							Debug.Log ("Found Up" + landList [x, y]);
							islandTag (x, y, 1, 1);
						}
					}
				}
			}
			border = findMax (border);

			if (border[1] > gridRow-2) {
				for (int cnt = 0; cnt < selectedIslands.Count; cnt++) {
					selectedIslands [cnt].XPos -= 2;
				}
				border [0]-= 2;
				border [1]-= 2;
				break;
			}
		}
	}



	void Awake () {
		cells = new HexCell[gridRow * gridColumn];
		gridCanvas = GetComponentInChildren<Canvas>();

		//Creating the Island
		initRandom(gridRow,gridColumn);


		//Indicating the Island with landCounter
		for (int cnt = 0; cnt < islands.Count; cnt++) {
			if (landList[islands[cnt].XPos,islands[cnt].YPos] == 0) {
				landCounter++;
				islandTag(islands[cnt].XPos,islands[cnt].YPos,landCounter,0);
			}	
			islands [cnt].No = landList [islands [cnt].XPos, islands [cnt].YPos];
			if (islands [cnt].No == 1) {
				selectedIslands.Add (new SelectedIslands(islands [cnt].XPos, islands [cnt].YPos,1));
			}
		}
			
		combineIslands();
			
		//Debug Log on Islands

		//for(int cnt = 0; cnt < islands.Count; cnt++) {
	//		Debug.Log("X: " + islands[cnt].XPos + "  Y:" + islands[cnt].YPos + "  No:" + islands[cnt].No);
	//	}
		/*for(int cnt = 0; cnt < selectedIslands.Count; cnt++) {
			Debug.Log("X: " + selectedIslands[cnt].XPos + "  Y:" + selectedIslands[cnt].YPos + "  No:" + selectedIslands[cnt].No);
		}*/
			

		//Creating the Cells
		for(int cnt = 0; cnt < selectedIslands.Count; cnt++) {
			CreateCell(selectedIslands[cnt].YPos,selectedIslands[cnt].XPos,cnt);
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