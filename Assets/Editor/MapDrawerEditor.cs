using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(MapDrawer))]
public class MapDrawerEditor : Editor {
  Map m;
  MapDrawer d;

  void OnEnable() {
    d = (MapDrawer) target;
    m = d.map;
  }

  public override void OnInspectorGUI() {
    DrawDefaultInspector();
    if (GUI.changed) { 
      m.plane = d.gameObject;
    }
  }
}
