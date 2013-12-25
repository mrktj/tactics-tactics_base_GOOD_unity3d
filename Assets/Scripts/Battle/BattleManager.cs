using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class BattleManager : MonoBehaviour {

  public Map map;
  public Camera cam;
  public Transform cursor;
  public Transform selection1;
  public Transform selection2;
  private HexCoord selection = null;
	// Use this for initialization
  
  void Start() {
  }
  
  void Update() {
    if (Network.isClient) {
      return;
    }
    DoMove(CursorHandler(Input.mousePosition, Input.GetMouseButtonUp(0), Input.GetMouseButtonUp(1)));
  }

  public void LoadBattle() {
    MapLoader.LoadFromXML(map, "xmlTest");
    map.DrawMap();
  }
	
  public void MoveCursor(Vector3 pos) {
    bool active = selection1.gameObject.activeSelf;
    if (active && pos == selection1.localPosition) {
      selection1.gameObject.SetActive(false);
    }
    else {
      selection1.gameObject.SetActive(true);
      selection1.localPosition = pos;
    }
  }

  public void DoMove(int[] data) {
    if (data != null) {
      map.MoveEntity(data[0], new HexCoord(data[1], data[2]));
    }
  }

  public int[] CursorHandler(Vector3 mousePos, bool left, bool right) {
    Vector3 newMouse = mousePos;
    newMouse.x = mousePos.x/Screen.width;
    newMouse.y = mousePos.y/Screen.height;
    Ray ray = cam.ViewportPointToRay(newMouse);
    RaycastHit hit = new RaycastHit();
    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8)) {
      HexCoord newSelection = map.gridDrawer.TexToHex(hit.textureCoord);
      if (newSelection != null) {
        Vector2 newPos = map.gridDrawer.HexToPixel(newSelection);
        if (newSelection != selection) {
          selection = newSelection;
          cursor.gameObject.SetActive(true);
          cursor.localPosition = newPos;
        }
        if (left) {
          MoveCursor(newPos);
        }
        if (right && selection1.gameObject.activeSelf) {
          if (selection1.gameObject.activeSelf && newPos != (Vector2) selection1.localPosition) {
            MapEntity m1 = map.EntityAt(selection1.gameObject.transform.localPosition);
            MapEntity m2 = map.EntityAt(newPos);

            if (m1 != null && m2 == null) {
              HexCoord h = map.gridDrawer.PixelToHex(newPos);
              int[] output = new int[3];
              output[0] = m1.idx;
              output[1] = h.i;
              output[2] = h.j;
              MoveCursor(newPos);
              return output;
            }
          }
        }
      }
    }
    else {
      cursor.gameObject.SetActive(false);
    }
    return null;
  }
}
