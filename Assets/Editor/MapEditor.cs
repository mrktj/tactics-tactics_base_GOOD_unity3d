using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor {
  [SerializeField]
  private int type = 0;

  private Map m;
  private bool update = true;
  private Texture[] images;
  private Tool lastTool = Tool.None;

  void OnEnable() {
    lastTool = Tools.current;
    Tools.current = Tool.None; 

    m = (Map) target;
    images = new Texture[m.numCelTypes];
    for (int i = 0; i < m.numCelTypes; i++) {
      Texture2D tex = new Texture2D(m.celSize, m.celSize);
      tex.SetPixels(m.template.GetPixels(i * m.celSize, 0, m.celSize, m.celSize));
      tex.Apply();
      images[i] = tex;
    }
  }

  void OnDisable() {
    Tools.current = lastTool;
  }

  public override void OnInspectorGUI() {
    Tools.current = Tool.None;

    DrawDefaultInspector();
    update = EditorGUILayout.Toggle("Redraw on Update", update);
    if(!update) {
      if (GUILayout.Button("Redraw Map")) {
        m.ResizeMap();
      }
    }
    else if (GUI.changed) {
      m.ResizeMap();
    }
  }

  public void OnSceneGUI () {
    Tools.current = Tool.None;

    type = GUILayout.SelectionGrid(type, images, m.numCelTypes);
    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
    if (Event.current.type == EventType.MouseDrag && Event.current.button == 0) {
      Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
      RaycastHit hit = new RaycastHit();
      if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8)
          && hit.transform == m.plane.gameObject.transform) {
        HexCoord cel = m.GetCoordFromTex(hit.textureCoord);
        m.SetCel(cel, type);
        m.ColorMap();
      }
    }
    HandleUtility.Repaint();
  }
}
