using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class HexGrid {
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

  public HexGrid(int i, int j) { 
    _width = i;
    _height = j;
      grid = new int[width * height];
  }

#endregion
#region Public Methods

  public int Cel(int i, int j) {
    return grid[i + j * width];
  }

  public bool SetCel(int i, int j, int val) {
    if (i >= 0 && i < width && j >=0 && j < height) {
      SetCel(i + j * width, val);
      return true;
    }
    return false;
  }

  public bool SetCel(int i, int val) {
    if (i >= 0 && i < width * height ) {
      grid[i] = val;
      return true;
    }
    return false;
  }

  public bool SetCel(HexCoord c, int val) {
    return SetCel(c.i, c.j, val);
  }

#endregion
#region Private Methods


#endregion 
}
