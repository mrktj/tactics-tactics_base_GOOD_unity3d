using UnityEngine;
using System;
using System.Collections;

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

#endregion
#region Accessors

  public HexGrid grid { get {return _grid;}}
  public int width {get {return _width;}}
  public int height {get {return _height;}}

#endregion
#region Delegates
#endregion
#region Unity Methods

  public void OnEnable() {
    if (_grid == null) {
      _grid = ScriptableObject.CreateInstance<HexGrid> ();
      _grid.ReSize(width, height);
    }
  }

  public void Init(int w, int h) {
    _width = w;
    _height = h;
  }

#endregion
#region Public Methods

  public void ResizeMap(int w, int h) {
    _width = w;
    _height = h;
    _grid.ReSize(width, height);
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
