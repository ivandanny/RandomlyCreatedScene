using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

	public HexCell cellPrefab;
	HexCell[] cells;

	public static int gridRow = 15;
	public static int gridColumn = 15;
	[Range (0.0f,1.0f)]
	public float initStart;
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
	void initRandom () {
		for (int i=0; i< initLand; i++) {
			int x = Random.Range (0,gridRow);
			int y = Random.Range (0,gridColumn);
			if (gridList[x,y] == 1){
				i -= 1;
			}
			generateLand(x,y);
		} 
	}

	void Awake () {
		initLand = Mathf.RoundToInt(gridRow * gridColumn * initStart);
		Debug.Log (initLand);
		initRandom();
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