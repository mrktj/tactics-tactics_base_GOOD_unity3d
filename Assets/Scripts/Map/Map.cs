using UnityEngine;
using System;
using System.Collections;

[ExecuteInEditMode]
public class Map : MonoBehaviour {
#region Public Variables

  public int width = 0;
  public int height = 0;

  public int pixelHeightOffset = 16;
  public int celSize = 64;
  public Texture2D template;
  public GameObject plane;

#endregion 
#region PrivateVariables

  [SerializeField]
  private HexGrid _grid;

#endregion
#region Accessors

  public HexGrid grid { get {return _grid;}}
  public int numCelTypes { get {return template.width / celSize;}}

#endregion
#region Delegates

  public delegate void MapUpdateEvent();
  public event MapUpdateEvent OnMapUpdate;

#endregion
#region Unity Methods

  public void OnEnable() {
    _grid = ScriptableObject.CreateInstance<HexGrid> ();
    _grid.Init(width, height);
  }

#endregion
#region Public Methods

  public void ResizeMap() {
    _grid.ReSize(width, height);
    OnMapUpdate();
  }

  public void ColorMap() {
    OnMapUpdate();
  }

  public HexCoord GetCoordFromTex(Vector2 TextureCoord) {
    int pixelWidth = celSize * width + celSize / 2;
    int pixelHeight = (celSize - pixelHeightOffset) * (height - 1) + celSize;
    float x = ((1 - TextureCoord.x) * pixelWidth) ;
    float y = (TextureCoord.y * pixelHeight) ;

    x = (x - celSize / 2) / celSize;
    float t1 = y / (celSize / 2);
    float t2 = Mathf.Floor(x + t1);
    float r = Mathf.Floor((Mathf.Floor(t1 - x) + t2) / 3);
    float q = Mathf.Floor((Mathf.Floor(2 * x + 1) + t2) / 3) - r;

    return new HexCoord((int) q, (int) r, true);
  }

  public void SetCel(HexCoord h, int val) {
    _grid.SetCel(h, val);
  }

  public int Cel(HexCoord h) {
    return _grid.Cel(h.i, h.j);
  }

  public int Cel(int i, int j) {
    return _grid.Cel(i, j);
  }

#endregion
}
