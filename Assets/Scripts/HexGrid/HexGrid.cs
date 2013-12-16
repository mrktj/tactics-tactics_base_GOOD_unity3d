using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class HexGrid : ScriptableObject {
#region Public Variables
  
#endregion
#region Private Variables

  private int _width;
  private int _height;
  private int[] grid;

#endregion
#region Accessors

  public int width {get {return _width;}}
  public int height {get {return _height;}}

#endregion
#region UnityFunctions

  public void OnEnable() { 
    hideFlags = HideFlags.HideAndDontSave;
  }

#endregion
#region Public Methods

  public void Init(int i, int j) {
    _width = i;
    _height = j;
    grid = new int[width * height];
  }

  public int Cel(int i, int j) {
    return grid[i + j * width];
  }

  public void SetCel(int i, int j, int val) {
    grid[i + j * width] = val;
  }

  public void SetCel(HexCoord c, int val) {
    grid[c.i + c.j * width] = val;
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
