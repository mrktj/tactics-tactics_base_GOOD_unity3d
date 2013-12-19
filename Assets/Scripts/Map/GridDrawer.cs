using UnityEngine;
using System;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class GridDrawer: MonoBehaviour {
#region Public Variables

  public int width = 0;
  public int height = 0;
  public int heightOffset = 16;
  public int celSize = 64;
  public Texture2D template;

#endregion
#region Private Variables

  private Color[][] cels;
  [SerializeField]
  private Map map;

#endregion
#region Accessors
  
  public int pixelWidth {get {return celSize * width + celSize / 2;}}
  public int pixelHeight {get {return (celSize - heightOffset) * (height - 1) + celSize;}}
  public int numCelTypes {get {return template.width / celSize;}}

#endregion
#region Unity Methods

  void OnEnable() {
    cels = new Color[numCelTypes][];
    for (int i = 0; i < numCelTypes; i++) {
      cels[i] = template.GetPixels(i * celSize, 0, celSize, celSize);
    }

    if (map == null) {
      map = ScriptableObject.CreateInstance<Map>();
      map.Init(width, height, this);

      this.gameObject.renderer.sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));
      this.gameObject.renderer.sharedMaterial.mainTexture = new Texture2D(pixelWidth, pixelHeight);
      this.gameObject.renderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(-1, -1));
      GenerateTexture();

      Unit u = ScriptableObject.CreateInstance<Unit> ();
      u.Init("Sword");
      map.SpawnMapEntity(u.CreateMapEntity(new HexCoord(5, 5)));
    }
  }

  void OnDisable() {
  }

	void Start () {
	}

#endregion
#region Public Methods

  public void ReApply() {
    map.ResizeMap(width, height);
  }

  public void GenerateTexture() {
    map.ResizeMap(width, height);

    Texture2D tex = new Texture2D(pixelWidth, pixelHeight);
    tex.Resize(pixelWidth, pixelHeight);
    tex.filterMode = FilterMode.Point;

    // Scale Quad to be pixel perfect
    this.gameObject.transform.localScale = 
      new Vector3(pixelWidth * .01f, pixelHeight * .01f, 1);
    
    // Color texture properly
    Color[] cols = new Color[pixelWidth * pixelHeight];
    for (int i = 0; i < map.width; i++) {
      for (int j = 0; j < map.height; j++) {
        int x0 = (celSize / 2) * (j % 2) + i * celSize;
        int y0 = pixelHeight - 1 - j * (celSize - heightOffset);
        int idx = map.Cel(i, j);
        for (int x = 0; x < celSize; x++) { 
          for (int y = 0; y < celSize; y++) {
            Color c = cels[idx][(celSize - 1 - x) + y * celSize];
            if (c.a != 0) {
              cols[(x0 + x) + (y0 -  y) * pixelWidth] = c;
            }
          }
        }
      }
    }
    tex.SetPixels(cols);
    tex.Apply();
    this.gameObject.renderer.sharedMaterial.mainTexture = tex;
  }

  public Vector2 HexToPixel(HexCoord h) { 
    Vector2 scale = this.gameObject.transform.localScale ;
    Vector2 origin = new Vector2(- 0.5f + celSize/200.0f/scale.x, 0.5f - celSize/200.0f/scale.y);
    Vector2 offset = new Vector2((celSize * h.i + celSize / 2 * (h.j & 1)), 
                       - (celSize / 2) * 3.0f / 2 * h.j);
    offset.x /= scale.x * 100.0f;
    offset.y /= scale.y * 100.0f;
    return offset + origin;
  }

  public HexCoord TexToHex(Vector2 TextureCoord) {
    float x = (TextureCoord.x * pixelWidth);
    float y = ((1 - TextureCoord.y) * pixelHeight);

    x = (x - celSize / 2) / celSize;
    float t1 = y / (celSize / 2);
    float t2 = Mathf.Floor(x + t1);
    float r = Mathf.Floor((Mathf.Floor(t1 - x) + t2) / 3);
    float q = Mathf.Floor((Mathf.Floor(2 * x + 1) + t2) / 3) - r;
    
    HexCoord h = new HexCoord((int) q, (int) r, true);
    return h;
  }

  public void ColorCel(HexCoord idx, int val) {
    if (!map.SetCel(idx, val)) {
      return;
    }

    Texture2D tex = (Texture2D) this.gameObject.renderer.sharedMaterial.mainTexture;

    int x0 = (celSize / 2) * ((height - 1 - idx.j) % 2) + (width - 1 - idx.i) * celSize;
    int y0 = pixelHeight - 1 - (height - 1 - idx.j) * (celSize - heightOffset);
    for (int x = 0; x < celSize; x++) { 
      for (int y = 0; y < celSize; y++) {
        if (val == numCelTypes - 1) {
          Color c = cels[0][(celSize - 1 - x) + y * celSize];
          if (c.a != 0) {
            tex.SetPixel(x0 + x, (y0 - y) , Color.clear);
          }
        }
        else {
          Color c = cels[val][(celSize - 1 - x) + y * celSize];
          if (c.a != 0) {
            tex.SetPixel(x0 + x, (y0 - y) , c);
          }
        }
      }
    }
    tex.Apply();
  }

#endregion
#region Private Methods

#endregion
}
