using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

	public HexCell cellPrefab;
	HexCell[] cells;

	public static int gridRow = 30;
	public static int gridColumn = 30;
	public float initStart = 0.55f;
	public float zoom = 0.3f;
	public float shiftIndicator = 0.3f;
	private int initLand = 0;
	public int[,] gridList = new int[gridRow,gridColumn];
	private string result;
	public HexMesh hexMesh;

	public Text cellLabelPrefab;
	Canvas gridCanvas;

	void generateLand(int heightLoc, int widthLoc) {
		gridList[heightLoc, widthLoc] = 1; 
	}
	
	// Use this for initialization
	void initRandom (int row, int column) {
		
		Vector2 shift = new Vector2(shiftIndicator,shiftIndicator); // play with this to shift map around
		int offset = Random.Range (0, 1000);
		for(int x = offset; x < (column+offset); x++)
			for(int y = offset; y < (row+offset); y++)
		{
			Vector2 pos = zoom * (new Vector2(x,y)) + shift;
			Vector2 pos2 = pos*4;
			float noise = Mathf.PerlinNoise(pos.x, pos.y);
			noise += (Mathf.PerlinNoise ((pos2.x), (pos2.y))-0.5f)/2.5f;
			if (noise<(1-initStart)) gridList[(x-offset),(y-offset)] = 0;
			else {
				gridList[(x-offset),(y-offset)] = 1; // land
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
		Debug.Log (initLand);
		initRandom(gridRow,gridColumn);
		cells = new HexCell[gridRow * gridColumn];

		gridCanvas = GetComponentInChildren<Canvas>();
		
		for (int z = 0, i = 0; z < gridRow; z++) {
			for (int x = 0; x < gridColumn; x++) {
				if (gridList[z,x] == 1) {
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
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = x.ToString() + "\n" + z.ToString();
	}

	
}