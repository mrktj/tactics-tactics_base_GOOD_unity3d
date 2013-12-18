using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(MapDrawer))]
public class MapDrawerEditor : Editor {
  [SerializeField]
  private int type = 0;

  private MapDrawer m;
  private bool update = true;
  private GUIContent[] guiIcons;
  private Tool lastTool = Tool.None;
  private bool editing = false;

  void OnEnable() {
    m = (MapDrawer) target;
    guiIcons = new GUIContent[m.numCelTypes];
    for (int i = 0; i < m.numCelTypes; i++) {
      Texture2D tex = new Texture2D(m.celSize, m.celSize);
      tex.SetPixels(m.template.GetPixels(i * m.celSize, 0, m.celSize, m.celSize));
      tex.Apply();
      guiIcons[i] = new GUIContent(tex);
    }
    m.ReApply();
  }

  void OnDisable() {
  }

  public override void OnInspectorGUI() {
    DrawDefaultInspector();
    update = EditorGUILayout.Toggle("Redraw on Update", update);
    if(!update) {
      if (GUILayout.Button("Redraw Map")) {
        m.GenerateTexture();
      }
    }
    else if (GUI.changed) {
      m.GenerateTexture();
    }
  }

  public void OnSceneGUI () {
    Handles.BeginGUI();
    editing = GUILayout.Toggle(editing, "Edit");

    EditorUtility.SetSelectedWireframeHidden(m.gameObject.renderer, editing);
    Tools.current = Tool.None;
    if (!editing) {
      Tools.current = (Tool) Tools.viewTool;
    }
    if (editing) {
      HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

      type = GUILayout.SelectionGrid(type, guiIcons, m.numCelTypes);
      if (Event.current.type == EventType.MouseDrag && Event.current.button == 0) {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8)
            && hit.transform == m.gameObject.transform) {
          HexCoord cel = m.GetCoordFromTex(hit.textureCoord);
          m.ColorCel(cel, type);
        }
      }
      HandleUtility.Repaint();
    }
    Handles.EndGUI();
  }
}
