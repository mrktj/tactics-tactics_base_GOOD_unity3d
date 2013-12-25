using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {
#region Public Variables

#endregion 
#region PrivateVariables

  [SerializeField]
  private HexGrid _grid;
  [SerializeField]
  private List<MapEntity> _mapEntities;
  [SerializeField]
  private GridDrawer _gridDrawer;

#endregion
#region Accessors

  public GridDrawer gridDrawer {get {return _gridDrawer;}}
  public HexGrid grid { get {return _grid;}}
  public List<MapEntity> mapEntities {get {return _mapEntities;}}
  public int width {get {return grid.width;}}
  public int height {get {return grid.width;}}

#endregion
#region Delegates
#endregion
#region Unity Methods

  public void OnEnable() {
    _mapEntities = new List<MapEntity>();
  }

#endregion
#region Public Methods

  public void DrawMap() {
    DrawGrid();
  }

  public void DrawGrid() {
    gridDrawer.DrawGrid();
  }

  public void MoveEntity(int idx, HexCoord h) {
    mapEntities[idx].MoveTo(h);
  }

  public MapEntity EntityWithIdx(int idx) {
    return mapEntities[idx];
  }

  public MapEntity EntityAt(HexCoord h) {
    foreach (MapEntity me in _mapEntities) {
      if (me.pos == h) {
        return me;
      }
    }
    return null;
  }

  public MapEntity EntityAt(Vector3 v) {
    HexCoord h = gridDrawer.PixelToHex(v);
    return EntityAt(h);
  }

  public bool RemoveEntity(MapEntity m0) {
    foreach (MapEntity m1 in _mapEntities) {
      if (m1 == m0) {
        DestroyImmediate(m1.gameObject);
        _mapEntities.Remove(m1);
        return true;
      }
    }
    return false;
  }

  public bool RemoveEntityAt(HexCoord h) {
    foreach (MapEntity me in _mapEntities) {
      if (me.pos == h) {
        DestroyImmediate(me.gameObject);
        _mapEntities.Remove(me);
        return true;
      }
    }
    return false;
  }
  
  public void SpawnMapEntity(HexCoord h, Entity e, bool friendly) {
    MapEntity me = e.CreateMapEntity(h, this, friendly);
    _mapEntities.Add(me);
  }

  public void Load(HexGrid g, Texture2D template) {
    _grid = g;
    _gridDrawer = new GridDrawer(template, g, this.gameObject);
  }

#endregion
}
