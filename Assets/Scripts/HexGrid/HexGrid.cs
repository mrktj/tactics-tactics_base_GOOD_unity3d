using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class HexGrid : ScriptableObject {
#region Public Variables
  
#endregion
#region Private Variables

  [SerializeField]
  private int _width;
  [SerializeField]
  private int _height;
  [SerializeField]
  private int[] grid;

#endregion
#region Accessors

  public int width {get {return _width;}}
  public int height {get {return _height;}}

#endregion
#region Constructors
  public HexGrid() {
    _width = 0;
    _height = 0;
    grid = new int[width * height];
  }

  public void OnEnable() { 
    hideFlags = HideFlags.HideAndDontSave;
    _width = 0;
    _height = 0;
    grid = new int[width * height];
  }

#endregion
#region Public Methods

  public int Cel(int i, int j) {
    return grid[i + j * width];
  }

  public bool SetCel(int i, int j, int val) {
    if (i >= 0 && i < width && j >=0 && j < height) {
      grid[i + j * width] = val;
      return true;
    }
    return false;
  }

  public bool SetCel(HexCoord c, int val) {
    return SetCel(c.i, c.j, val);
  }

  public void ReSize(int i, int j) {
    _width = i;
    _height = j;
    Array.Resize<int>(ref grid, i + j * _width);
  }

#endregion
#region Private Methods


#endregion 
}
