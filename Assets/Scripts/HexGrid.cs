using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour {

	public Material[] material_list;

	public HexCell cellPrefab;
	HexCell[] cells;
	private string result;
	public HexMesh hexMesh;

	//Grid Function Variable
	public static int gridColumn = 20;
	public static int gridRow = 20;

	//Perlin Funcion Variable
	public int[,] gridList = new int[gridRow,gridColumn];
	public float[,] heightList = new float[gridRow, gridColumn];
	public float initStart = 0.55f;
	public float zoom = 0.3f;
	public float shiftIndicator = 0.3f;

	//Variable for Creating Countries
	public int minIslands = 5;
	public int maxIslands = 10;

	//Variable for Countries Label
	public Text cellLabelPrefab;
	Canvas gridCanvas;


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

		//Is to add the current land to the SelectedIslands group.
		if (selected == 1) {
			selectedIslands.Add (new SelectedIslands(row, column,1));
		}
		landList [row, column] = counter;
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
		//int offset = Random.Range (0, 10000);
		int offset = 19;
		for(int x = offset; x < (row+offset); x++)
			for(int y = offset; y < (column+offset); y++)
			{
				Vector2 pos = zoom * (new Vector2(x,y)) + shift;
				Vector2 pos2 = pos*4;
				Vector2 pos3 = pos / 3;
				float noise = Mathf.PerlinNoise(pos.x, pos.y);
				noise += (Mathf.PerlinNoise ((pos2.x), (pos2.y))-0.5f)/2.5f;
				heightList [x - offset, y - offset] = noise;
				noise += (Mathf.PerlinNoise ((pos3.x), (pos3.y)))/1.5f;
				if (noise<(1.8-initStart)) gridList[(x-offset),(y-offset)] = 0;
				else {
					gridList[x-offset, y-offset] = 1;
					islands.Add (new Islands(x-offset,y-offset,0));
				}
			}
	}

	int[] findMax ( int [] corner) {
		corner [0]  = selectedIslands[0].XPos;
		corner [1]  = selectedIslands[0].XPos;
		corner [2]  = selectedIslands[0].YPos;
		corner [3]  = selectedIslands[0].YPos;
		for(int cnt = 0; cnt < selectedIslands.Count; cnt++) {
			if (selectedIslands [cnt].XPos < corner [0] )
				corner [0]  = selectedIslands [cnt].XPos;
			if (selectedIslands [cnt].XPos > corner [1] )
				corner [1]  = selectedIslands [cnt].XPos;
			if (selectedIslands [cnt].YPos < corner [2] )
				corner [2]  = selectedIslands [cnt].YPos;
			if (selectedIslands [cnt].YPos > corner [3] )
				corner [3]  = selectedIslands [cnt].YPos;
		}
		return (corner);
	}

	//Move the first island to connect with the other island
	void combineIslands () {
		bool canMove = false;
		int[] border = { 0, 0, 0, 0 };
		int x = selectedIslands [0].XPos;
		int y = selectedIslands [0].YPos;
		border = findMax (border);
		int loopCheck = 0;

		while (border [1] < (gridRow - 1) || canMove == false) {  // Lok for this statement!
			loopCheck += 1;
			if (loopCheck > 7) {
				return;
			}
			canMove = true;
			//Go Right
			while (border [3] < (gridColumn - 2)) {
				for (int cnt = 0; cnt < selectedIslands.Count; cnt++) {
					selectedIslands [cnt].YPos += 1;
					x = selectedIslands [cnt].XPos;
					y = selectedIslands [cnt].YPos;
					if (y + 1 < gridColumn) {
						if (gridList [x, y] == 1 && landList [x, y] != 1) {
							islandTag (x, y, 1, 1);
						}
					}
				}
				border = findMax (border);
				if (border [1] >= gridRow) {
					for (int cnt = 0; cnt < selectedIslands.Count; cnt++) {
						selectedIslands [cnt].YPos -= 1;
					}
					border [0]--;
					border [1]--;
				}
			}
			//Go Left
			while (border [2] > 0) {
				for (int cnt = 0; cnt < selectedIslands.Count; cnt++) {
					selectedIslands [cnt].YPos -= 1;
					x = selectedIslands [cnt].XPos;
					y = selectedIslands [cnt].YPos;
					if (y > 0) {
						if (gridList [x, y] == 1 && landList [x, y] != 1) {
							islandTag (x, y, 1, 1);
						}
					}
				}
				border = findMax (border);
				if (border [2] < 0) {
					for (int cnt = 0; cnt < selectedIslands.Count; cnt++) {
						selectedIslands [cnt].YPos += 1;
					}
					border [2]++;
					border [3]++;
				}
			}

			//Go Up
			int first = selectedIslands.Count;
			int second = 0;

			if (border [1] < gridRow - 2) {
				for (int cnt = 0; cnt < selectedIslands.Count; cnt++) {
					selectedIslands [cnt].XPos += 2;
					x = selectedIslands [cnt].XPos;
					y = selectedIslands [cnt].YPos;
					if (x < gridRow) {
						if (gridList [x, y] == 1 && landList [x, y] != 1) {
							islandTag (x, y, 1, 1);
							second = selectedIslands.Count;
							for (int i = second-1; i >= first; i-- ) {
								if (selectedIslands [i].XPos >= 2) {
									selectedIslands [i].XPos -= 2;
								}
							}
						}
					}
				}
			}
			border = findMax (border);

			if (border [1] > gridRow - 2 && border[0] >= 2) {
				for (int cnt = 0; cnt < selectedIslands.Count; cnt++) {
					selectedIslands [cnt].XPos -= 2;
				}
				border [0] -= 2;
				border [1] -= 2;
				return;
			}
		}

	}
		
	void labelCountry (int row, int column, int size, int mark, int no) {
		if (landList[row,column] != 0 || no >= size || gridList[row,column] != 1){
			return;
		}

		landList [row, column] = mark;

		if (no < size) {
			landList [row, column] = mark;
			int[] search = indexCheck (row, column);

			if (row %2 == 0) {
				if (search [0] == 1) {
					if (gridList [row + 1, column] <= 0 || (landList[row+1,column] <= mark || landList[row+1,column] != 0)) {
						search [0] = 0;
					}
				}
				if (search [1] == 1) {
					if (gridList [row , column+1] <= 0|| (landList[row,column+1] == mark || landList[row,column+1] != 0)) {
						search [1] = 0;
					}
				}
				if (search [2] == 1) {
					if (gridList [row -1, column] <= 0|| (landList[row-1,column] == mark || landList[row-1,column] != 0)) {
						search [2] = 0;
					}
				}
				if (search [3] == 1) {
					if (gridList [row -1, column-1] <= 0|| (landList[row-1,column-1] == mark || landList[row-1,column-1] != 0)) {
						search [3] = 0;
					}
				}
				if (search [4] == 1) {
					if (gridList [row , column-1] <= 0|| (landList[row,column-1] == mark || landList[row,column-1] != 0)) {
						search [4] = 0;
					}
				}
				if (search [5] == 1) {
					if (gridList [row + 1, column-1] <= 0|| (landList[row+1,column-1] == mark || landList[row+1,column-1] != 0)) {
						search [5] = 0;
					}
				}
			}
			if (row %2 == 1) {
				if (search [0] == 1) {
					if (gridList [row + 1, column+1] <= 0|| (landList[row+1,column+1] == mark || landList[row+1,column+1] != 0)) {
						search [0] = 0;
					}
				}
				if (search [1] == 1) {
					if (gridList [row , column+1] <= 0|| (landList[row,column+1] == mark || landList[row,column+1] != 0)) {
						search [1] = 0;
					}
				}
				if (search [2] == 1) {
					if (gridList [row -1, column+1] <= 0|| (landList[row-1,column+1] == mark || landList[row-1,column+1] != 0)) {
						search [2] = 0;
					}
				}
				if (search [3] == 1) {
					if (gridList [row -1, column] <= 0|| (landList[row-1,column] == mark || landList[row-1,column] != 0)) {
						search [3] = 0;
					}
				}
				if (search [4] == 1) {
					if (gridList [row , column-1] <= 0|| (landList[row,column-1] == mark || landList[row,column-1] != 0)) {
						search [4] = 0;
					}
				}
				if (search [5] == 1) {
					if (gridList [row + 1, column] <= 0|| (landList[row+1,column] == mark || landList[row+1,column] != 0)) {
						search [5] = 0;
					}
				}
			}

			int sum = 0;
			int line = -1;
			for (int i = 0; i < search.Length; i++) {
				if (search [i] == 1) {
					
					sum++;
					//Debug.Log ("Sample :" + i);
				}
			}

			//Debug.Log (row + " " + column + ", " + mark +", sum: " + sum);

			if (sum != 0) {
				float chance = Random.Range (0.0f, 1.0f);
				int j = 0;
				while (chance > 0.0f) {
					j += 1;
					chance -= 1.0f / sum;
				}
				bool check = false;
				int k = 0;
				while ( check == false) {
					if (search [k] == 1) {
						j -= 1;
					}
					if (j == 0) {
						line = k;
						check = true;
					}
					k++;
				}

				//Debug.Log (line);
				//Check position by sum here (using Case)
				if (row %2 == 0) {
					switch (line) {
					case 0:
						//Debug.Log (mark + " is going top right, pos " + row + " " + column);
						labelCountry (row + 1, column, size, mark, (no + 1));
						break;
					case 1:
						//Debug.Log (mark + " is going right, pos " + row + " " + column);
						labelCountry (row , column + 1, size, mark, (no+1));
						break;
					case 2:
						//Debug.Log (mark + " is going bottom right, pos " + row + " " + column);
						labelCountry (row - 1, column, size, mark, (no+1));
						break;
					case 3:
						//Debug.Log (mark + " is going bottom left, pos " + row + " " + column);
						labelCountry (row - 1, column - 1, size, mark, (no+1));
						break;
					case 4:
						//Debug.Log (mark + " is going left, pos " + row + " " + column);
						labelCountry (row, column - 1, size, mark, (no+1));
						break;
					case 5:
						//Debug.Log (mark + " is going top left, pos " + row + " " + column);
						labelCountry (row+1, column - 1, size, mark, (no+1));
						break;
					}
				}
				if (row %2 == 1) {
					switch (line) {
					case 0:
						//Debug.Log (mark + " is going top right, pos " + row + " " + column);
						labelCountry (row+1, column + 1, size, mark, (no+1));
						break;
					case 1:
						//Debug.Log (mark + " is going right, pos " + row + " " + column);
						labelCountry (row , column + 1, size, mark, (no+1));
						break;
					case 2:
						//Debug.Log (mark + " is going bottom right, pos " + row + " " + column);
						labelCountry (row - 1, column + 1, size, mark, (no+1));
						break;
					case 3:
						//Debug.Log (mark + " is going bottom left, pos " + row + " " + column);
						labelCountry (row - 1, column, size, mark, (no+1));
						break;
					case 4:
						//Debug.Log (mark + " is going left, pos " + row + " " + column);
						labelCountry (row, column - 1, size, mark, (no+1));
						break;
					case 5:
						//Debug.Log (mark + " is going top left, pos " + row + " " + column);
						labelCountry (row + 1, column, size, mark, (no+1));
						break;
					}
				}
			}
		}
	}
		
	void createCountries () {
		for(int cnt = 0; cnt < selectedIslands.Count; cnt++) {
			gridList [selectedIslands [cnt].XPos, selectedIslands [cnt].YPos] = 1;
		}

		int count2 = 0;
		for(int cnt = 0; cnt < selectedIslands.Count; cnt++) {
			int size = Random.Range(minIslands,maxIslands+1);
			int x = selectedIslands [cnt].XPos;
			int y = selectedIslands [cnt].YPos;
			if (landList [x, y] == 0) {
				count2++;
				labelCountry (x, y,size, count2, 0);
				size = Random.Range(minIslands,maxIslands+1);
			}
		}

	}



	void Awake () {

		gridCanvas = GetComponentInChildren<Canvas>();
		cells = new HexCell[gridRow * gridColumn];

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

		//Combine the separated islands into one	
		combineIslands();

		//Create Countries
		landList= new int[gridRow,gridColumn];
		gridList= new int[gridRow, gridColumn];
		createCountries();


//Creating the Cells
		for(int cnt = 0; cnt < selectedIslands.Count; cnt++) {
			CreateCell(selectedIslands[cnt].YPos,selectedIslands[cnt].XPos,cnt);
		}
	}


	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		//position.y = heightList[x,z]* 20;
		position.y = 0;
		position.z = z * (HexMetrics.outerRadius * 1.5f);
		
		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;

		HexMesh hex = Instantiate<HexMesh>(hexMesh);
		hex.transform.SetParent(transform, false);
		hex.transform.localPosition = position;

		hex.GetComponent<Renderer> ().material = material_list[landList[z,x] % material_list.Length];
		/*
		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition3D = new Vector3(position.x, position.z, (position.y *-1));
		label.text = landList[z,x].ToString();
		*/
	}


}