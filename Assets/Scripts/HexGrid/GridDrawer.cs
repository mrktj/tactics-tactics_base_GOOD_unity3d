using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class GridDrawer {
#region Public Variables


#endregion
#region Private Variables

  [SerializeField]
  private HexGrid grid; 
  [SerializeField]
  private Texture2D template;
  [SerializeField]
  private GameObject quad;

#endregion
#region Accessors
 
  private int _heightOffset = 16;
  private int _celSize = 64;
  public int width          {get {return grid.width;}}
  public int height         {get {return grid.height;}}
  public int celSize        {get {return _celSize;}}
  public int heightOffset   {get {return _heightOffset;}}
  public int pixelWidth     {get {return celSize * width + celSize / 2;}}
  public int pixelHeight    {get {return (celSize - heightOffset) * (height - 1) + celSize;}}
  public int numCelTypes    {get {return template.width / celSize;}}
  public Vector2 origin     {get {Vector2 o= (Vector2) quad.transform.localPosition;
                                  o -= new Vector2(pixelWidth/2 * 0.01f, -pixelHeight/2 * 0.01f);
                                  o += new Vector2(celSize/2 * 0.01f, -celSize/2 * 0.01f);
                                  return o;}}

#endregion
#region Unity Methods

  public GridDrawer(Texture2D temp, HexGrid hgrid, GameObject go) {
    grid = hgrid;
    template = temp;
    quad = go;
    go.renderer.sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));
    go.renderer.sharedMaterial.mainTexture = new Texture2D(pixelWidth, pixelHeight);
    go.renderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(1, 1));
    // Scale Quad to be pixel perfect
    quad.transform.localScale = 
      new Vector3(pixelWidth * .01f, pixelHeight * .01f, 1);
  }

#endregion
#region Public Methods

  public void DrawGrid() {
    Color[][] cels = new Color[numCelTypes][];
    for (int i = 0; i < numCelTypes; i++) {
      cels[i] = template.GetPixels(i * celSize, 0, celSize, celSize);
    }

    Texture2D tex = (Texture2D)  quad.renderer.sharedMaterial.mainTexture;
    tex.Resize(pixelWidth, pixelHeight);
    tex.filterMode = FilterMode.Point;

    
    // Color texture properly
    Color[] cols = new Color[pixelWidth * pixelHeight];
    for (int i = 0; i < width; i++) {
      for (int j = 0; j < height; j++) {
        int idx = grid.Cel(i, j);
        int x0 = (celSize / 2) * (j % 2) + i * celSize;
        int y0 = pixelHeight - 1 - j * (celSize - heightOffset);
        for (int x = 0; x < celSize; x++) { 
          for (int y = 0; y < celSize; y++) {
            Color c = cels[idx][(x) + (celSize - 1 - y) * celSize];
            if (c.a != 0) {
              cols[(x0 + x) + (y0 -  y) * pixelWidth] = c;
            }
          }
        }
      }
    }
    tex.SetPixels(cols);
    tex.Apply();
    quad.renderer.sharedMaterial.mainTexture = tex;
  }

  public void ColorCel(HexCoord idx, int val) {
    Texture2D tex = (Texture2D) quad.renderer.sharedMaterial.mainTexture;
    Color[] cels;
    if (val == numCelTypes - 1) {
      cels = template.GetPixels(0, 0, celSize, celSize);
    }
    else {
      cels = template.GetPixels(val * celSize, 0, celSize, celSize);
    }

    int x0 = (celSize / 2) * ((height - 1 - idx.j) % 2) + (width - 1 - idx.i) * celSize;
    int y0 = pixelHeight - 1 - (height - 1 - idx.j) * (celSize - heightOffset);
    for (int x = 0; x < celSize; x++) { 
      for (int y = 0; y < celSize; y++) {
        Color c = cels[x + (celSize - 1 - y) * celSize];
        if ((val == numCelTypes - 1) && (c.a != 0)) {
          tex.SetPixel(x0 + x, (y0 - y) , Color.clear);
        }
        else if (c.a != 0) {
          tex.SetPixel(x0 + x, (y0 - y) , c);
        }
      }
    }
    tex.Apply();
  }

  public Vector2 HexToPixel(HexCoord h) { 
    Vector2 offset = new Vector2((celSize * h.i + celSize / 2 * (h.j & 1)), 
                       - (celSize / 2) * 3.0f / 2 * h.j);
    offset.x *= 0.01f;
    offset.y *= 0.01f;
    return offset + origin;
  }

  public HexCoord PixelToHex(Vector2 pos) {
    Vector2 scale = quad.transform.localScale;
    float x = (pos.x - origin.x) * 100.0f + celSize/2;
    float y = pixelHeight - (pos.y + origin.y) * 100.0f - celSize/2;
    
    x = (x - celSize / 2) / celSize;
    float t1 = y / (celSize / 2);
    float t2 = Mathf.Floor(x + t1);
    float r = Mathf.Floor((Mathf.Floor(t1 - x) + t2) / 3);
    float q = Mathf.Floor((Mathf.Floor(2 * x + 1) + t2) / 3) - r;
    
    HexCoord h = new HexCoord((int) q, (int) r, true);
    if (h.i < 0 || h.i >= grid.width || h.j < 0 || h.j >= grid.height) {
      return null;
    }
    return h;
  }

  public HexCoord PixelToHex(Vector3 pos) {
    return PixelToHex((Vector2) pos);
  }

  public HexCoord TexToHex(Vector2 TextureCoord) {
    float x = (TextureCoord.x * pixelWidth);
    float y = ((1 - TextureCoord.y) * pixelHeight);

    x = (x - celSize / 2) / celSize;
    float t1 = y / (celSize / 2);
    float t2 = Mathf.Floor(x + t1);
    int r = (int) Mathf.Floor((Mathf.Floor(t1 - x) + t2) / 3);
    int q = (int) Mathf.Floor((Mathf.Floor(2 * x + 1) + t2) / 3) - r;
    
    HexCoord h = new HexCoord((int) q, (int) r, true);
    if (h.i < 0 || h.i >= grid.width || h.j < 0 || h.j >= grid.height) {
      return null;
    }
    return h;
  }

#endregion
#region Private Methods

#endregion
}
