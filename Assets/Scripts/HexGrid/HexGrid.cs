﻿using UnityEngine;
using System.Collections;

public class HexGrid {
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
#region Constructors

  public HexGrid() {
    _width = 0;
    _height = 0;
    grid = new int[width * height];
  }

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

  public void SetCel(int i, int j, int val) {
    grid[i + j * width] = val;
    Debug.Log(grid[i + j * width]);
  }

#endregion
#region Private Methods


#endregion 
}
