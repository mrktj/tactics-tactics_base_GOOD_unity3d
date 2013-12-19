using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Map : ScriptableObject {
#region Public Variables
  

#endregion 
#region PrivateVariables

  [SerializeField]
  private HexGrid _grid;
  [SerializeField]
  private int _width;
  [SerializeField]
  private int _height;
  [SerializeField]
  private List<MapEntity> _mapEntities;
  [SerializeField]
  private GridDrawer _drawer;

#endregion
#region Accessors

  public HexGrid grid { get {return _grid;}}
  public int width {get {return _width;}}
  public int height {get {return _height;}}
  public List<MapEntity> mapEntities {get {return _mapEntities;}}
  public GridDrawer drawer {get {return _drawer;}}

#endregion
#region Delegates
#endregion
#region Unity Methods

  public void OnEnable() {
    if (_grid == null) {
      _grid = ScriptableObject.CreateInstance<HexGrid> ();
      _grid.ReSize(width, height);
      _mapEntities = new List<MapEntity>();
    }
  }

  public void Init(int w, int h, GridDrawer d) {
    _width = w;
    _height = h;
    _drawer = d;
  }

#endregion
#region Public Methods

  public void ResizeMap(int w, int h) {
    _width = w;
    _height = h;
    _grid.ReSize(width, height);
  }

  public void SpawnMapEntity(MapEntity me) {
    _mapEntities.Add(me);
    GameObject go = (GameObject) Instantiate(Resources.Load("MapEntities/" + me.mapEntityName));
    Debug.Log(drawer);
    go.transform.parent = drawer.gameObject.transform;
    Debug.Log(drawer.HexToPixel(me.pos));
    go.transform.localPosition = drawer.HexToPixel(me.pos);
  }

  public bool SetCel(HexCoord h, int val) {
    return _grid.SetCel(h, val);
  }

  public int Cel(HexCoord h) {
    return _grid.Cel(h.i, h.j);
  }

  public int Cel(int i, int j) {
    return _grid.Cel(i, j);
  }

#endregion
}
