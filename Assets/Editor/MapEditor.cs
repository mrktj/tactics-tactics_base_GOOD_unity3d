using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor {
  Map m;
  bool update = true;
  int type = 0;
  Texture[] images;

  void OnEnable() {
    m = (Map) target;
    images = new Texture[m.numCelTypes];
    for (int i = 0; i < m.numCelTypes; i++) {
      Texture2D tex = new Texture2D(m.celSize, m.celSize);
      tex.SetPixels(m.template.GetPixels(i * m.celSize, 0, m.celSize, m.celSize));
      tex.Apply();
      images[i] = tex;
    }
  }

  public override void OnInspectorGUI() {
    DrawDefaultInspector();
    update = EditorGUILayout.Toggle("Redraw on Update", update);
    if (update && GUI.changed) { 
      m.ResizeMap();
    }
  }

  public void OnSceneGUI () {
    type = GUILayout.SelectionGrid(type, images, m.numCelTypes);
    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
    if (Event.current.type == EventType.MouseDown) {
      Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
      RaycastHit hit = new RaycastHit();
      if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8)) {
        HexCoord cel = m.GetCoordFromTex(hit.textureCoord);
        m.SetCel(cel, type);
        m.ColorMap();
      }
    }
    HandleUtility.Repaint();
  }
}
