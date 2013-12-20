using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class BattleManager : MonoBehaviour {

  public Map map;
  public Camera cam;
  public GameObject cursor;
  public Transform selection1;
  public Transform selection2;

  private HexCoord selection = null;
	// Use this for initialization

	void Start () {
    selection = new HexCoord(-1, -1);
    MapLoader.LoadFromFile(map, "TestMap");
    map.DrawMap();
	}
	
	// Update is called once per frame
	void Update () {
    bool active1 = selection1.gameObject.activeSelf;
    bool active2 = selection2.gameObject.activeSelf;
    if (active1 && active2) {
      MapEntity m1 = map.EntityAt(selection1.gameObject.transform.position);
      MapEntity m2 = map.EntityAt(selection2.gameObject.transform.position);

      if (m1 != null && m2 == null) {
        m1.MoveTo(map.gridDrawer.PixelToHex(selection2.gameObject.transform.position));
      }
    }
    
    CursorHandler();
	}

  public void CursorHandler() {
    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit = new RaycastHit();
    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8)) {
      HexCoord newSelection = map.gridDrawer.TexToHex(hit.textureCoord);
      Debug.Log(newSelection);
      Vector2 newPos = map.gridDrawer.HexToPixel(newSelection);
      if (newSelection != selection) {
        cursor.SetActive(true);
        selection = newSelection;
        cursor.transform.localPosition = newPos;
      }
      if (Input.GetMouseButtonUp(0)) {
        bool active = selection1.gameObject.activeSelf;
        if (active && newPos == (Vector2) selection1.localPosition) {
          selection1.gameObject.SetActive(false);
        }
        else { 
          selection1.gameObject.SetActive(true);
          selection1.localPosition = map.gridDrawer.HexToPixel(selection);
        }
      }
      if (Input.GetMouseButtonUp(1)) {
        bool active = selection2.gameObject.activeSelf;
        if (active && newPos == (Vector2) selection2.localPosition) {
          selection2.gameObject.SetActive(false);
        }
        else {
          selection2.gameObject.SetActive(true);
          selection2.localPosition = map.gridDrawer.HexToPixel(selection);
        }
      }
    }
  }
}
